using Demo.Infrastructure.Dto;

namespace Demo.Infrastructure.Tools;

/// <summary>
/// 通用单据状态机。
/// 迁移图显式声明各状态允许的下一状态，非法迁移抛 Biz_Exception。
/// 每种单据由一组状态常量和一个 <see cref="State_Map"/> 组成，
/// 新增单据只需声明常量与迁移图。
/// </summary>
public static class Order_State_Machine
{
    /// <summary>
    /// 状态迁移图：Key = 当前状态，Value = 允许迁移到的下一批状态。
    /// </summary>
    public class State_Map
    {
        private readonly Dictionary<int, int[]> _map = new();
        private readonly string _name;

        public State_Map(string name) => _name = name;

        /// <summary>声明「当前状态 → 允许的目标状态」迁移边。</summary>
        public State_Map Allow(int from, params int[] to)
        {
            _map[from] = to;
            return this;
        }

        /// <summary>当前状态是否允许迁移到目标状态。</summary>
        public bool CanTransit(int? current, int next)
        {
            var from = current ?? 0;
            return _map.TryGetValue(from, out var allow) && Array.IndexOf(allow, next) >= 0;
        }

        /// <summary>执行迁移；非法迁移抛业务异常。</summary>
        public int Transit(int? current, int next)
        {
            var from = current ?? 0;
            if (!CanTransit(from, next))
                throw new Biz_Exception($"{_name}不允许从状态 {from} 变更为 {next}");
            return next;
        }

        /// <summary>某状态下允许迁移到的目标状态集合。</summary>
        public IReadOnlyList<int> NextStates(int? current)
            => _map.TryGetValue(current ?? 0, out var allow) ? allow : Array.Empty<int>();
    }

    /// <summary>
    /// 销售订单状态机：新建 → 已确认 → 已发货 → 已完成
    /// </summary>
    public static class So
    {
        public const sbyte New = 0;
        public const sbyte Confirmed = 1;
        public const sbyte Shipped = 2;
        public const sbyte Completed = 3;

        public static readonly State_Map Map = new State_Map("销售订单")
            .Allow(New, Confirmed)
            .Allow(Confirmed, Shipped)
            .Allow(Shipped, Completed);

        public static sbyte Transit(sbyte? current, sbyte next) => (sbyte)Map.Transit(current, next);
    }

    /// <summary>
    /// 采购订单状态机：待审核 → 已审核 → 部分收货 → 已完成
    /// </summary>
    public static class Po
    {
        public const sbyte Pending = 0;
        public const sbyte Approved = 1;
        public const sbyte Partial = 2;
        public const sbyte Completed = 3;

        public static readonly State_Map Map = new State_Map("采购订单")
            .Allow(Pending, Approved)
            .Allow(Approved, Partial, Completed)
            .Allow(Partial, Completed);

        public static sbyte Transit(sbyte? current, sbyte next) => (sbyte)Map.Transit(current, next);
    }

    /// <summary>
    /// 生产订单状态机：计划 → 已下达 → 生产中 → 已完工 → 已关闭
    /// </summary>
    public static class Pr
    {
        public const sbyte Plan = 0;
        public const sbyte Released = 1;
        public const sbyte Producing = 2;
        public const sbyte Done = 3;
        public const sbyte Closed = 4;

        public static readonly State_Map Map = new State_Map("生产订单")
            .Allow(Plan, Released)
            .Allow(Released, Producing, Closed)
            .Allow(Producing, Done, Closed)
            .Allow(Done, Closed);

        public static sbyte Transit(sbyte? current, sbyte next) => (sbyte)Map.Transit(current, next);
    }

    /// <summary>
    /// 会计凭证状态机：录入 → 已审核 → 已记账 → 已结账
    /// </summary>
    public static class Voucher
    {
        public const sbyte Draft = 0;
        public const sbyte Audited = 1;
        public const sbyte Posted = 2;
        public const sbyte Closed = 3;

        public static readonly State_Map Map = new State_Map("凭证")
            .Allow(Draft, Audited)
            .Allow(Audited, Posted)
            .Allow(Posted, Closed);

        public static sbyte Transit(sbyte? current, sbyte next) => (sbyte)Map.Transit(current, next);
    }

    /// <summary>
    /// 库存盘点单状态机：盘点中 → 已完成 → 已审核；审批驳回可退回盘点中重新录入。
    /// </summary>
    public static class StockCheck
    {
        public const sbyte Checking = 0;
        public const sbyte Finished = 1;
        public const sbyte Audited = 2;

        public static readonly State_Map Map = new State_Map("盘点单")
            .Allow(Checking, Finished)
            .Allow(Finished, Audited, Checking);

        public static sbyte Transit(sbyte? current, sbyte next) => (sbyte)Map.Transit(current, next);
    }

    /// <summary>
    /// 审批单状态机：草稿 → 审批中 → 已通过 / 已驳回；已驳回可再次提交。
    /// </summary>
    public static class Approval
    {
        public const sbyte Draft = 0;
        public const sbyte Auditing = 1;
        public const sbyte Approved = 2;
        public const sbyte Rejected = 3;

        public static readonly State_Map Map = new State_Map("审批单")
            .Allow(Draft, Auditing)
            .Allow(Auditing, Approved, Rejected)
            .Allow(Rejected, Auditing);

        public static sbyte Transit(sbyte? current, sbyte next) => (sbyte)Map.Transit(current, next);
    }
}
