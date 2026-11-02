using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto;
using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Tools;
using Demo.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Demo.Service.Instance;

/// <summary>
/// 通用审批流服务实现。
/// ------------------------------------------------------------------
/// 状态流转（Order_State_Machine.Approval）：
///   草稿(0) ─提交→ 审批中(1) ─通过→ 已通过(2)
///                            └驳回→ 已驳回(3) ─再次提交→ 审批中(1)
///
/// 多节点：totalNodes &gt; 1 时，每个节点需分别审批；
///         节点号达到 totalNodes 才判定整单通过。
/// </summary>
[Inject_Attribute]
public class Approval_Service : I_Approval_Service
{
    private readonly ErpDbContext _db;
    private readonly BillNo_Helper _no;
    private readonly Redis_Cache _cache;
    private readonly I_Inventory_Service _inv;

    public Approval_Service(ErpDbContext db, BillNo_Helper no, Redis_Cache cache, I_Inventory_Service inv)
    {
        _db = db;
        _no = no;
        _cache = cache;
        _inv = inv;
    }

    public async Task<object> SubmitAsync(Approval_Submit_Request req)
    {
        if (string.IsNullOrWhiteSpace(req.title)) throw new Biz_Exception("审批标题不能为空");
        // 按规则自动计算审批级数 + 节点角色（覆盖前端传入的 totalNodes）
        var (lvl, nodeRoles) = await ResolveRuleAsync(req.bizType, req.amount);

        var bill = new SysApproval
        {
            ApprovalNo = await _no.NextAsync("AP"),
            BizType = req.bizType,
            BizId = req.bizId,
            BillNo = req.billNo,
            Title = req.title,
            Amount = req.amount,
            Status = Order_State_Machine.Approval.Draft,
            CurrentNode = 0,
            TotalNodes = lvl,
            NodeRoles = nodeRoles,
            Remark = req.remark,
            CreateTime = DateTime.Now
        };
        _db.SysApprovals.Add(bill);
        await _db.SaveChangesAsync();

        // 提交动作：草稿 → 审批中
        bill.Status = Order_State_Machine.Approval.Transit(bill.Status, Order_State_Machine.Approval.Auditing);
        bill.SubmitTime = DateTime.Now;
        _db.SysApprovalLogs.Add(new SysApprovalLog
        {
            ApprovalId = bill.Id,
            NodeNo = 0,
            Action = 1,
            ApproverName = "申请人",
            Opinion = req.remark,
            OperTime = DateTime.Now
        });
        await _db.SaveChangesAsync();
        await _cache.RemoveAsync("erp:approval:pending");
        return bill;
    }

    public async Task<object> HandleAsync(long id, Approval_Handle_Request req, string? approverRole, string? approverName)
    {
        await using var tx = await _db.Database.BeginTransactionAsync();
        var bill = (await _db.SysApprovals
            .FromSqlInterpolated($"SELECT * FROM sys_approval WHERE id = {id} FOR UPDATE")
            .ToListAsync()).FirstOrDefault()
            ?? throw new Biz_Exception("审批单不存在");
        if (bill.Status != Order_State_Machine.Approval.Auditing)
            throw new Biz_Exception("该审批单当前状态不可审批");

        // 节点角色权限校验：当前待审节点要求的角色需与审批人角色匹配（admin 放行）
        if (!string.IsNullOrWhiteSpace(bill.NodeRoles))
        {
            var roles = bill.NodeRoles.Split(',');
            var need = roles.Length > bill.CurrentNode ? roles[bill.CurrentNode] : "admin";
            if (!string.IsNullOrWhiteSpace(approverRole) && approverRole != "admin" && need != "admin" && need != approverRole)
                throw new Biz_Exception($"当前节点需【{need}】角色审批，您无权审批");
        }

        var action = req.action == 3 ? (sbyte)3 : (sbyte)2;
        var target = action == 3
            ? Order_State_Machine.Approval.Rejected
            : Order_State_Machine.Approval.Approved;

        // 通过且还有后续节点 → 停留在审批中，仅推进节点号
        var isFinalPass = action == 2 && bill.CurrentNode + 1 < bill.TotalNodes;

        if (!isFinalPass)
            bill.Status = Order_State_Machine.Approval.Transit(bill.Status, target);

        bill.CurrentNode += 1;
        if (bill.Status is Order_State_Machine.Approval.Approved or Order_State_Machine.Approval.Rejected)
            bill.FinishTime = DateTime.Now;

        _db.SysApprovalLogs.Add(new SysApprovalLog
        {
            ApprovalId = bill.Id,
            NodeNo = bill.CurrentNode,
            Action = action,
            ApproverName = string.IsNullOrWhiteSpace(approverName) ? "审批人" : approverName,
            Opinion = req.opinion,
            OperTime = DateTime.Now
        });
        await _db.SaveChangesAsync();

        try
        {
            if (bill.Status == Order_State_Machine.Approval.Approved)
                await ApplyToBizAsync(bill);
            else if (bill.Status == Order_State_Machine.Approval.Rejected)
                await RollbackBizAsync(bill);
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }

        await _cache.RemoveAsync("erp:approval:pending");
        await tx.CommitAsync();
        return bill;
    }

    public async Task<object> CancelAsync(long id, string? reason)
    {
        var bill = await _db.SysApprovals.FindAsync(id) ?? throw new Biz_Exception("审批单不存在");
        if (bill.Status is not (Order_State_Machine.Approval.Draft or Order_State_Machine.Approval.Rejected))
            throw new Biz_Exception("仅草稿或已驳回的审批单可撤回");

        bill.Status = Order_State_Machine.Approval.Transit(bill.Status, Order_State_Machine.Approval.Draft);
        bill.CurrentNode = 0;
        bill.FinishTime = null;
        _db.SysApprovalLogs.Add(new SysApprovalLog
        {
            ApprovalId = bill.Id,
            NodeNo = bill.CurrentNode,
            Action = 4,
            ApproverName = "申请人",
            Opinion = reason,
            OperTime = DateTime.Now
        });
        await _db.SaveChangesAsync();
        await _cache.RemoveAsync("erp:approval:pending");
        return bill;
    }

    public async Task<object> PageAsync(Page_Request req, sbyte? status)
    {
        var q = _db.SysApprovals.AsQueryable();
        if (status.HasValue) q = q.Where(d => d.Status == status.Value);
        if (!string.IsNullOrWhiteSpace(req.keyword))
            q = q.Where(d => d.ApprovalNo.Contains(req.keyword)
                         || d.Title.Contains(req.keyword)
                         || (d.BillNo ?? "").Contains(req.keyword));

        var total = await q.CountAsync();
        var rows = await q.OrderByDescending(d => d.Id)
            .Skip((req.SafePage - 1) * req.SafeSize).Take(req.SafeSize).ToListAsync();
        return new Demo.Infrastructure.Dto.Response.Page_Result { total = total, rows = rows };
    }

    public async Task<object> DetailAsync(long id)
    {
        var bill = await _db.SysApprovals.FindAsync(id) ?? throw new Biz_Exception("审批单不存在");
        var logs = await _db.SysApprovalLogs.Where(d => d.ApprovalId == id)
            .OrderBy(d => d.Id).ToListAsync();
        return new { approval = bill, logs };
    }

    public async Task<object> PendingCountAsync()
    {
        var cacheKey = "erp:approval:pending";
        var cached = await _cache.GetAsync<int>(cacheKey);
        if (cached > 0) return cached;

        var count = await _db.SysApprovals.CountAsync(d => d.Status == Order_State_Machine.Approval.Auditing);
        await _cache.SetAsync(cacheKey, count, TimeSpan.FromMinutes(1));
        return count;
    }

    public async Task<object> GetByBizAsync(string bizType, long bizId)
    {
        var bill = await _db.SysApprovals
            .Where(d => d.BizType == bizType && d.BizId == bizId)
            .OrderByDescending(d => d.Id)
            .FirstOrDefaultAsync();
        if (bill == null) return null;

        var logs = await _db.SysApprovalLogs.Where(d => d.ApprovalId == bill.Id)
            .OrderBy(d => d.Id).ToListAsync();
        return new { approval = bill, logs };
    }

    /// <summary>
    /// 审批通过后回写业务单据状态：
    ///   PO → 采购订单转「已审核」
    ///   SO → 销售订单转「已确认」
    /// 回写同样走状态机，非法迁移会被拦截。
    /// </summary>
    private async Task ApplyToBizAsync(SysApproval bill)
    {
        if (bill.BizId == null) return;

        switch (bill.BizType)
        {
            case "PO":
                {
                    var po = await _db.PoOrders.FindAsync(bill.BizId.Value);
                    if (po != null && po.Status == Order_State_Machine.Po.Pending)
                        po.Status = Order_State_Machine.Po.Transit(po.Status, Order_State_Machine.Po.Approved);
                    break;
                }
            case "SO":
                {
                    var so = await _db.SoOrders.FindAsync(bill.BizId.Value);
                    if (so != null && so.Status == Order_State_Machine.So.New)
                        so.Status = Order_State_Machine.So.Transit(so.Status, Order_State_Machine.So.Confirmed);
                    break;
                }
            case "REQUISITION":
                {
                    var r = await _db.PoRequisitions.FindAsync(bill.BizId.Value);
                    if (r != null) r.AuditStatus = 1;
                    break;
                }
            case "RECEIPT":
                {
                    var r = await _db.PoReceipts.FindAsync(bill.BizId.Value);
                    if (r != null) r.AuditStatus = 1;
                    break;
                }
            case "PO_CHANGE":
                {
                    var po = await _db.PoOrders.FindAsync(bill.BizId.Value);
                    if (po != null) po.AuditStatus = 1;
                    break;
                }
            case "QUOTE":
                {
                    var q = await _db.SoQuotes.FindAsync(bill.BizId.Value);
                    if (q != null) q.AuditStatus = 1;
                    break;
                }
            case "CREDIT":
                {
                    var c = await _db.SoCustomerCredits.FindAsync(bill.BizId.Value);
                    if (c != null) c.AuditStatus = 1;
                    break;
                }
            case "VOUCHER":
                {
                    var v = await _db.FinVouchers.FindAsync(bill.BizId.Value);
                    if (v != null) v.AuditStatus = 1;
                    break;
                }
            case "FIN_RECEIPT":
                {
                    var r = await _db.FinReceipts.FindAsync(bill.BizId.Value);
                    if (r != null) r.AuditStatus = 1;
                    break;
                }
            case "FIN_PAYMENT":
                {
                    var p = await _db.FinPayments.FindAsync(bill.BizId.Value);
                    if (p != null) p.AuditStatus = 1;
                    break;
                }
            case "IN_ORDER":
                {
                    var o = await _db.InvInOrders.FindAsync(bill.BizId.Value);
                    if (o != null) o.AuditStatus = 1;
                    break;
                }
            case "OUT_ORDER":
                {
                    var o = await _db.InvOutOrders.FindAsync(bill.BizId.Value);
                    if (o != null) o.AuditStatus = 1;
                    break;
                }
            case "TRANSFER":
                {
                    var t = await _db.InvTransfers.FindAsync(bill.BizId.Value);
                    if (t != null) t.AuditStatus = 1;
                    break;
                }
            case "CHECK":
                {
                    var c = await _db.InvChecks.FindAsync(bill.BizId.Value);
                    if (c != null)
                    {
                        // 审批通过才按差异调整库存（盘盈入 / 盘亏出）并置为已审核
                        if (c.Status == Order_State_Machine.StockCheck.Finished)
                            await _inv.AuditCheckAsync(bill.BizId.Value);
                        c.AuditStatus = 1;
                    }
                    break;
                }
        }
        await _db.SaveChangesAsync();
    }

    /// <summary>审批被驳回时回写业务单据（采购订单退回待审核）。</summary>
    private async Task RollbackBizAsync(SysApproval bill)
    {
        if (bill.BizId == null) return;

        if (bill.BizType == "PO")
        {
            var po = await _db.PoOrders.FindAsync(bill.BizId.Value);
            if (po != null && po.Status == Order_State_Machine.Po.Approved)
                po.Status = Order_State_Machine.Po.Pending;
        }
        else if (bill.BizType == "SO")
        {
            var so = await _db.SoOrders.FindAsync(bill.BizId.Value);
            if (so != null && so.Status == Order_State_Machine.So.Confirmed)
                so.Status = Order_State_Machine.So.New;
        }
        else if (bill.BizType == "REQUISITION")
        {
            var r = await _db.PoRequisitions.FindAsync(bill.BizId.Value);
            if (r != null) r.AuditStatus = 2;
        }
        else if (bill.BizType == "RECEIPT")
        {
            var r = await _db.PoReceipts.FindAsync(bill.BizId.Value);
            if (r != null) r.AuditStatus = 2;
        }
        else if (bill.BizType == "PO_CHANGE")
        {
            var po = await _db.PoOrders.FindAsync(bill.BizId.Value);
            if (po != null) po.AuditStatus = 2;
        }
        else if (bill.BizType == "QUOTE")
        {
            var q = await _db.SoQuotes.FindAsync(bill.BizId.Value);
            if (q != null) q.AuditStatus = 2;
        }
        else if (bill.BizType == "CREDIT")
        {
            var c = await _db.SoCustomerCredits.FindAsync(bill.BizId.Value);
            if (c != null) c.AuditStatus = 2;
        }
        else if (bill.BizType == "VOUCHER")
        {
            var v = await _db.FinVouchers.FindAsync(bill.BizId.Value);
            if (v != null) v.AuditStatus = 2;
        }
        else if (bill.BizType == "FIN_RECEIPT")
        {
            var r = await _db.FinReceipts.FindAsync(bill.BizId.Value);
            if (r != null) r.AuditStatus = 2;
        }
        else if (bill.BizType == "FIN_PAYMENT")
        {
            var p = await _db.FinPayments.FindAsync(bill.BizId.Value);
            if (p != null) p.AuditStatus = 2;
        }
        else if (bill.BizType == "IN_ORDER")
        {
            var o = await _db.InvInOrders.FindAsync(bill.BizId.Value);
            if (o != null) o.AuditStatus = 2;
        }
        else if (bill.BizType == "OUT_ORDER")
        {
            var o = await _db.InvOutOrders.FindAsync(bill.BizId.Value);
            if (o != null) o.AuditStatus = 2;
        }
        else if (bill.BizType == "TRANSFER")
        {
            var t = await _db.InvTransfers.FindAsync(bill.BizId.Value);
            if (t != null) t.AuditStatus = 2;
        }
        else if (bill.BizType == "CHECK")
        {
            var c = await _db.InvChecks.FindAsync(bill.BizId.Value);
            if (c != null)
            {
                // 驳回：退回盘点中，不调整库存
                await _inv.RejectCheckAsync(bill.BizId.Value);
                c.AuditStatus = 2;
            }
        }
        await _db.SaveChangesAsync();
    }

    /// <summary>按业务类型+金额匹配审批规则，返回（级数, 节点角色序列）。无匹配则默认 1 级、admin 审批。</summary>
    private async Task<(int level, string nodeRoles)> ResolveRuleAsync(string bizType, decimal? amount)
    {
        var amt = amount ?? 0;
        var rule = await _db.SysApprovalRules
            .Where(r => r.BizType == bizType && (r.Enabled ?? 1) != 0
                        && amt >= r.MinAmount && (r.MaxAmount == null || amt < r.MaxAmount))
            .OrderByDescending(r => r.MinAmount)
            .FirstOrDefaultAsync();
        if (rule == null) return (1, "admin");
        return (rule.Level, rule.NodeRoles ?? "admin");
    }

    /// <summary>审批规则列表（按业务类型过滤）</summary>
    public async Task<object> GetRulesAsync(string? bizType)
    {
        var q = _db.SysApprovalRules.AsQueryable();
        if (!string.IsNullOrWhiteSpace(bizType)) q = q.Where(r => r.BizType == bizType);
        var rows = await q.OrderBy(r => r.BizType).ThenBy(r => r.MinAmount).ToListAsync();
        return new { total = rows.Count, rows };
    }

    /// <summary>保存（新增/更新）审批规则</summary>
    public async Task<object> SaveRuleAsync(Approval_Rule_Save_Request req)
    {
        if (string.IsNullOrWhiteSpace(req.bizType)) throw new Biz_Exception("请选择业务类型");
        if (req.level < 1) throw new Biz_Exception("审批级数至少为1");
        if (req.id.HasValue && req.id.Value > 0)
        {
            var e = await _db.SysApprovalRules.FindAsync(req.id.Value) ?? throw new Biz_Exception("规则不存在");
            e.BizType = req.bizType; e.MinAmount = req.minAmount; e.MaxAmount = req.maxAmount;
            e.Level = req.level; e.NodeRoles = req.nodeRoles; e.Enabled = req.enabled ?? 1; e.Remark = req.remark;
        }
        else
        {
            _db.SysApprovalRules.Add(new SysApprovalRule
            {
                BizType = req.bizType, MinAmount = req.minAmount, MaxAmount = req.maxAmount,
                Level = req.level, NodeRoles = req.nodeRoles, Enabled = req.enabled ?? 1,
                Remark = req.remark, CreateTime = DateTime.Now
            });
        }
        await _db.SaveChangesAsync();
        return "ok";
    }

    /// <summary>删除审批规则</summary>
    public async Task<object> DeleteRuleAsync(long id)
    {
        var e = await _db.SysApprovalRules.FindAsync(id) ?? throw new Biz_Exception("规则不存在");
        _db.SysApprovalRules.Remove(e);
        await _db.SaveChangesAsync();
        return "ok";
    }
}
