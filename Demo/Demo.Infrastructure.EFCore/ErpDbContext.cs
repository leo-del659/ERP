using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Demo.Infrastructure.EFCore;

public class ErpDbContext : DbContext
{
    public ErpDbContext()
    {
    }

    public ErpDbContext(DbContextOptions<ErpDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FinCost> FinCosts { get; set; }

    public virtual DbSet<FinPayable> FinPayables { get; set; }

    public virtual DbSet<FinPayment> FinPayments { get; set; }

    public virtual DbSet<FinReceipt> FinReceipts { get; set; }

    public virtual DbSet<FinReceivable> FinReceivables { get; set; }

    public virtual DbSet<FinVoucher> FinVouchers { get; set; }

    public virtual DbSet<FinVoucherEntry> FinVoucherEntries { get; set; }

    public virtual DbSet<InvBatch> InvBatches { get; set; }

    public virtual DbSet<InvCheck> InvChecks { get; set; }

    public virtual DbSet<InvCheckDetail> InvCheckDetails { get; set; }

    public virtual DbSet<InvInDetail> InvInDetails { get; set; }

    public virtual DbSet<InvInOrder> InvInOrders { get; set; }

    public virtual DbSet<InvOutDetail> InvOutDetails { get; set; }

    public virtual DbSet<InvOutOrder> InvOutOrders { get; set; }

    public virtual DbSet<InvStock> InvStocks { get; set; }

    public virtual DbSet<InvTransaction> InvTransactions { get; set; }

    public virtual DbSet<InvTransfer> InvTransfers { get; set; }

    public virtual DbSet<MdAccount> MdAccounts { get; set; }

    public virtual DbSet<MdBom> MdBoms { get; set; }

    public virtual DbSet<MdCustomer> MdCustomers { get; set; }

    public virtual DbSet<MdMaterial> MdMaterials { get; set; }

    public virtual DbSet<MdSupplier> MdSuppliers { get; set; }

    public virtual DbSet<MdWarehouse> MdWarehouses { get; set; }

    public virtual DbSet<PoOrder> PoOrders { get; set; }

    public virtual DbSet<PoOrderDetail> PoOrderDetails { get; set; }

    public virtual DbSet<PoReceipt> PoReceipts { get; set; }

    public virtual DbSet<PoReceiptDetail> PoReceiptDetails { get; set; }

    public virtual DbSet<PoReturn> PoReturns { get; set; }

    public virtual DbSet<PoSupplierQuote> PoSupplierQuotes { get; set; }

    public virtual DbSet<PrMaterialIssue> PrMaterialIssues { get; set; }

    public virtual DbSet<PrMaterialReturn> PrMaterialReturns { get; set; }

    public virtual DbSet<PrMp> PrMps { get; set; }

    public virtual DbSet<PrMrp> PrMrps { get; set; }

    public virtual DbSet<PrOrder> PrOrders { get; set; }

    public virtual DbSet<PrOutsourcing> PrOutsourcings { get; set; }

    public virtual DbSet<PrOutsourcingIssue> PrOutsourcingIssues { get; set; }

    public virtual DbSet<PrReport> PrReports { get; set; }

    public virtual DbSet<RptKpi> RptKpis { get; set; }

    public virtual DbSet<RptReport> RptReports { get; set; }

    public virtual DbSet<SoCustomerCredit> SoCustomerCredits { get; set; }

    public virtual DbSet<SoDelivery> SoDeliveries { get; set; }

    public virtual DbSet<SoDeliveryDetail> SoDeliveryDetails { get; set; }

    public virtual DbSet<SoOrder> SoOrders { get; set; }

    public virtual DbSet<SoOrderDetail> SoOrderDetails { get; set; }

    public virtual DbSet<SoReturn> SoReturns { get; set; }

    public virtual DbSet<SysDeptRole> SysDeptRoles { get; set; }

    public virtual DbSet<SysDeptRoleMenu> SysDeptRoleMenus { get; set; }

    public virtual DbSet<SysMenu> SysMenus { get; set; }

    public virtual DbSet<SysOperLog> SysOperLogs { get; set; }

    public virtual DbSet<SysRole> SysRoles { get; set; }

    public virtual DbSet<SysUser> SysUsers { get; set; }

    public virtual DbSet<SysUserRole> SysUserRoles { get; set; }

    public virtual DbSet<SysExLog> SysExLogs { get; set; }
    public virtual DbSet<PayNotify> PayNotifies { get; set; }

    public virtual DbSet<SysApproval> SysApprovals { get; set; }
    public virtual DbSet<SysApprovalLog> SysApprovalLogs { get; set; }
    public virtual DbSet<SysApprovalRule> SysApprovalRules { get; set; }

    public virtual DbSet<PoRequisition> PoRequisitions { get; set; }
    public virtual DbSet<PoRequisitionDetail> PoRequisitionDetails { get; set; }

    public virtual DbSet<SoQuote> SoQuotes { get; set; }
    public virtual DbSet<SoQuoteDetail> SoQuoteDetails { get; set; }
    public virtual DbSet<SoDeliveryTrack> SoDeliveryTracks { get; set; }

    public virtual DbSet<MdLocation> MdLocations { get; set; }

    public virtual DbSet<SysBillChange> SysBillChanges { get; set; }
    public virtual DbSet<MesRouting> MesRoutings { get; set; }
    public virtual DbSet<MesRoutingStep> MesRoutingSteps { get; set; }
    public virtual DbSet<MesQualityCheck> MesQualityChecks { get; set; }

    public virtual DbSet<MesWorkReport> MesWorkReports { get; set; }
    public virtual DbSet<MesMaterialIssue> MesMaterialIssues { get; set; }
    public virtual DbSet<MesProductionOrder> MesProductionOrders { get; set; }
    public virtual DbSet<MesWorkstation> MesWorkstations { get; set; }
    public virtual DbSet<MesWorkOrder> MesWorkOrders { get; set; }

    public virtual DbSet<HrDepartment> HrDepartments { get; set; }
    public virtual DbSet<HrPosition> HrPositions { get; set; }
    public virtual DbSet<HrPositionQuota> HrPositionQuotas { get; set; }
    public virtual DbSet<HrEmployee> HrEmployees { get; set; }
    public virtual DbSet<HrEmployeeChange> HrEmployeeChanges { get; set; }
    public virtual DbSet<HrContract> HrContracts { get; set; }
    public virtual DbSet<HrShift> HrShifts { get; set; }
    public virtual DbSet<HrSchedule> HrSchedules { get; set; }
    public virtual DbSet<HrAttendance> HrAttendances { get; set; }
    public virtual DbSet<HrLeave> HrLeaves { get; set; }
    public virtual DbSet<HrOvertime> HrOvertimes { get; set; }
    public virtual DbSet<HrSalaryItem> HrSalaryItems { get; set; }
    public virtual DbSet<HrEmployeeSalary> HrEmployeeSalarys { get; set; }
    public virtual DbSet<HrSalary> HrSalarys { get; set; }
    public virtual DbSet<HrSalaryDetail> HrSalaryDetails { get; set; }
    public virtual DbSet<HrSocialRule> HrSocialRules { get; set; }
    public virtual DbSet<HrTaxBracket> HrTaxBrackets { get; set; }
    public virtual DbSet<HrWelfare> HrWelfares { get; set; }
    public virtual DbSet<HrRecruitPlan> HrRecruitPlans { get; set; }
    public virtual DbSet<HrCandidate> HrCandidates { get; set; }
    public virtual DbSet<HrInterview> HrInterviews { get; set; }
    public virtual DbSet<HrOffer> HrOffers { get; set; }
    public virtual DbSet<HrPerfIndicator> HrPerfIndicators { get; set; }
    public virtual DbSet<HrPerfPlan> HrPerfPlans { get; set; }
    public virtual DbSet<HrPerfResult> HrPerfResults { get; set; }
    public virtual DbSet<HrPerfDetail> HrPerfDetails { get; set; }
    public virtual DbSet<HrTrainNeed> HrTrainNeeds { get; set; }
    public virtual DbSet<HrTrainPlan> HrTrainPlans { get; set; }
    public virtual DbSet<HrTrainRecord> HrTrainRecords { get; set; }
    public virtual DbSet<HrTrainExpense> HrTrainExpenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<FinCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("fin_cost", tb => tb.HasComment("成本计算单"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.CostNo, "uk_cost_no").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CalcDate)
                .HasComment("核算日期")
                .HasColumnName("calc_date");
            entity.Property(e => e.CostNo)
                .HasMaxLength(50)
                .HasComment("成本单号")
                .HasColumnName("cost_no");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.LaborCost)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("'0.00'")
                .HasComment("人工成本")
                .HasColumnName("labor_cost");
            entity.Property(e => e.MaterialCost)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("'0.00'")
                .HasComment("材料成本")
                .HasColumnName("material_cost");
            entity.Property(e => e.MaterialId)
                .HasComment("成品物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.OverheadCost)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("'0.00'")
                .HasComment("制造费用")
                .HasColumnName("overhead_cost");
            entity.Property(e => e.PrOrderId)
                .HasComment("生产订单ID")
                .HasColumnName("pr_order_id");
            entity.Property(e => e.TotalCost)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("'0.00'")
                .HasComment("总成本")
                .HasColumnName("total_cost");
            entity.Property(e => e.UnitCost)
                .HasPrecision(18, 6)
                .HasDefaultValueSql("'0.000000'")
                .HasComment("单位成本")
                .HasColumnName("unit_cost");
        });

        modelBuilder.Entity<FinPayable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("fin_payable", tb => tb.HasComment("应付单"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.SupplierId, "idx_pay_supplier");

            entity.HasIndex(e => e.PayNo, "uk_pay_no").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasPrecision(18, 2)
                .HasComment("应付金额")
                .HasColumnName("amount");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.DueDate)
                .HasComment("到期日")
                .HasColumnName("due_date");
            entity.Property(e => e.PaidAmount)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("'0.00'")
                .HasComment("已付金额")
                .HasColumnName("paid_amount");
            entity.Property(e => e.PayNo)
                .HasMaxLength(50)
                .HasComment("应付单号")
                .HasColumnName("pay_no");
            entity.Property(e => e.SourceOrderId)
                .HasComment("来源采购订单ID")
                .HasColumnName("source_order_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("0未付/1部分付/2已付")
                .HasColumnName("status");
            entity.Property(e => e.SupplierId)
                .HasComment("供应商ID")
                .HasColumnName("supplier_id");
        });

        modelBuilder.Entity<FinPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("fin_payment", tb => tb.HasComment("付款单"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.PaymentNo, "uk_payment_no").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasPrecision(18, 2)
                .HasComment("付款金额")
                .HasColumnName("amount");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.AuditStatus)
                .HasDefaultValueSql("'0'")
                .HasComment("审批状态：0待审/1通过/2驳回")
                .HasColumnName("audit_status");
            entity.Property(e => e.PayableId)
                .HasComment("核销应付单ID")
                .HasColumnName("payable_id");
            entity.Property(e => e.PaymentDate)
                .HasComment("付款日期")
                .HasColumnName("payment_date");
            entity.Property(e => e.PaymentNo)
                .HasMaxLength(50)
                .HasComment("付款单号")
                .HasColumnName("payment_no");
            entity.Property(e => e.SupplierId)
                .HasComment("供应商ID")
                .HasColumnName("supplier_id");
        });

        modelBuilder.Entity<FinReceipt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("fin_receipt", tb => tb.HasComment("收款单"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.ReceiptNo, "uk_receipt_no").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasPrecision(18, 2)
                .HasComment("收款金额")
                .HasColumnName("amount");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.AuditStatus)
                .HasDefaultValueSql("'0'")
                .HasComment("审批状态：0待审/1通过/2驳回")
                .HasColumnName("audit_status");
            entity.Property(e => e.CustomerId)
                .HasComment("客户ID")
                .HasColumnName("customer_id");
            entity.Property(e => e.ReceiptDate)
                .HasComment("收款日期")
                .HasColumnName("receipt_date");
            entity.Property(e => e.ReceiptNo)
                .HasMaxLength(50)
                .HasComment("收款单号")
                .HasColumnName("receipt_no");
            entity.Property(e => e.ReceivableId)
                .HasComment("核销应收单ID")
                .HasColumnName("receivable_id");
        });

        modelBuilder.Entity<FinReceivable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("fin_receivable", tb => tb.HasComment("应收单"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.CustomerId, "idx_recv_customer");

            entity.HasIndex(e => e.RecvNo, "uk_recv_no").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasPrecision(18, 2)
                .HasComment("应收金额")
                .HasColumnName("amount");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.CustomerId)
                .HasComment("客户ID")
                .HasColumnName("customer_id");
            entity.Property(e => e.DueDate)
                .HasComment("到期日")
                .HasColumnName("due_date");
            entity.Property(e => e.ReceivedAmount)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("'0.00'")
                .HasComment("已收金额")
                .HasColumnName("received_amount");
            entity.Property(e => e.RecvNo)
                .HasMaxLength(50)
                .HasComment("应收单号")
                .HasColumnName("recv_no");
            entity.Property(e => e.SourceOrderId)
                .HasComment("来源销售订单ID")
                .HasColumnName("source_order_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("0未收/1部分收/2已收")
                .HasColumnName("status");
        });

        modelBuilder.Entity<FinVoucher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("fin_voucher", tb => tb.HasComment("财务凭证主表"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.VoucherDate, "idx_voucher_date");

            entity.HasIndex(e => e.VoucherNo, "uk_voucher_no").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.SourceId)
                .HasComment("来源单据ID")
                .HasColumnName("source_id");
            entity.Property(e => e.SourceModule)
                .HasMaxLength(20)
                .HasComment("来源模块：PO/SO/INV/PR")
                .HasColumnName("source_module");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("0未审核/1已审核")
                .HasColumnName("status");
            entity.Property(e => e.AuditStatus)
                .HasDefaultValueSql("'0'")
                .HasComment("审批状态：0待审/1通过/2驳回")
                .HasColumnName("audit_status");
            entity.Property(e => e.TotalCredit)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("'0.00'")
                .HasComment("贷方合计")
                .HasColumnName("total_credit");
            entity.Property(e => e.TotalDebit)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("'0.00'")
                .HasComment("借方合计")
                .HasColumnName("total_debit");
            entity.Property(e => e.VoucherDate)
                .HasComment("凭证日期")
                .HasColumnName("voucher_date");
            entity.Property(e => e.VoucherNo)
                .HasMaxLength(30)
                .HasComment("凭证号")
                .HasColumnName("voucher_no");
        });

        modelBuilder.Entity<FinVoucherEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("fin_voucher_entry", tb => tb.HasComment("凭证分录"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.VoucherId, "idx_entry_voucher");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountCode)
                .HasMaxLength(20)
                .HasComment("会计科目编码")
                .HasColumnName("account_code");
            entity.Property(e => e.Amount)
                .HasPrecision(18, 2)
                .HasComment("金额")
                .HasColumnName("amount");
            entity.Property(e => e.Direction)
                .HasMaxLength(4)
                .HasComment("方向：DR借/CR贷")
                .HasColumnName("direction");
            entity.Property(e => e.EntryNo)
                .HasComment("分录行号")
                .HasColumnName("entry_no");
            entity.Property(e => e.Memo)
                .HasMaxLength(200)
                .HasComment("摘要")
                .HasColumnName("memo");
            entity.Property(e => e.VoucherId)
                .HasComment("凭证ID")
                .HasColumnName("voucher_id");
        });

        modelBuilder.Entity<InvBatch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("inv_batch", tb => tb.HasComment("批次库存"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => new { e.MaterialId, e.BatchNo }, "idx_batch_mat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BatchNo)
                .HasMaxLength(50)
                .HasComment("批次号")
                .HasColumnName("batch_no");
            entity.Property(e => e.ExpiryDate)
                .HasComment("有效期")
                .HasColumnName("expiry_date");
            entity.Property(e => e.MaterialId)
                .HasComment("物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.ProductionDate)
                .HasComment("生产日期")
                .HasColumnName("production_date");
            entity.Property(e => e.Quantity)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("批次数量")
                .HasColumnName("quantity");
            entity.Property(e => e.WarehouseId)
                .HasComment("仓库ID")
                .HasColumnName("warehouse_id");
        });

        modelBuilder.Entity<InvCheck>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("inv_check", tb => tb.HasComment("盘点单"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.CheckNo, "uk_check_no").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CheckDate)
                .HasComment("盘点日期")
                .HasColumnName("check_date");
            entity.Property(e => e.CheckNo)
                .HasMaxLength(50)
                .HasComment("盘点单号")
                .HasColumnName("check_no");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("0盘点中/1已完成")
                .HasColumnName("status");
            entity.Property(e => e.AuditStatus)
                .HasDefaultValueSql("'0'")
                .HasComment("审批状态：0待审/1通过/2驳回")
                .HasColumnName("audit_status");
            entity.Property(e => e.WarehouseId)
                .HasComment("仓库ID")
                .HasColumnName("warehouse_id");
        });

        modelBuilder.Entity<InvCheckDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("inv_check_detail", tb => tb.HasComment("盘点明细"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.CheckId, "idx_cd_check");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActualQty)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("实盘数量")
                .HasColumnName("actual_qty");
            entity.Property(e => e.BookQty)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("账面数量")
                .HasColumnName("book_qty");
            entity.Property(e => e.CheckId)
                .HasComment("盘点单ID")
                .HasColumnName("check_id");
            entity.Property(e => e.DiffQty)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("差异数量")
                .HasColumnName("diff_qty");
            entity.Property(e => e.MaterialId)
                .HasComment("物料ID")
                .HasColumnName("material_id");
        });

        modelBuilder.Entity<InvInDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("inv_in_detail", tb => tb.HasComment("入库明细"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.InOrderId, "idx_ind_order");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.InOrderId)
                .HasComment("入库单ID")
                .HasColumnName("in_order_id");
            entity.Property(e => e.MaterialId)
                .HasComment("物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.Quantity)
                .HasPrecision(18, 4)
                .HasComment("入库数量")
                .HasColumnName("quantity");
        });

        modelBuilder.Entity<InvInOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("inv_in_order", tb => tb.HasComment("入库单"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.InNo, "uk_in_no").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.AuditStatus)
                .HasDefaultValueSql("'0'")
                .HasComment("审批状态：0待审/1通过/2驳回")
                .HasColumnName("audit_status");
            entity.Property(e => e.InDate)
                .HasComment("入库日期")
                .HasColumnName("in_date");
            entity.Property(e => e.InNo)
                .HasMaxLength(50)
                .HasComment("入库单号")
                .HasColumnName("in_no");
            entity.Property(e => e.InType)
                .HasMaxLength(20)
                .HasComment("类型：PO采购/PR生产/OTHER其他")
                .HasColumnName("in_type");
            entity.Property(e => e.SourceNo)
                .HasMaxLength(50)
                .HasComment("来源单据号")
                .HasColumnName("source_no");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("0待审/1已审")
                .HasColumnName("status");
            entity.Property(e => e.WarehouseId)
                .HasComment("仓库ID")
                .HasColumnName("warehouse_id");
        });

        modelBuilder.Entity<InvOutDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("inv_out_detail", tb => tb.HasComment("出库明细"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.OutOrderId, "idx_outd_order");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaterialId)
                .HasComment("物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.OutOrderId)
                .HasComment("出库单ID")
                .HasColumnName("out_order_id");
            entity.Property(e => e.Quantity)
                .HasPrecision(18, 4)
                .HasComment("出库数量")
                .HasColumnName("quantity");
        });

        modelBuilder.Entity<InvOutOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("inv_out_order", tb => tb.HasComment("出库单"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.OutNo, "uk_out_no").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.AuditStatus)
                .HasDefaultValueSql("'0'")
                .HasComment("审批状态：0待审/1通过/2驳回")
                .HasColumnName("audit_status");
            entity.Property(e => e.OutDate)
                .HasComment("出库日期")
                .HasColumnName("out_date");
            entity.Property(e => e.OutNo)
                .HasMaxLength(50)
                .HasComment("出库单号")
                .HasColumnName("out_no");
            entity.Property(e => e.OutType)
                .HasMaxLength(20)
                .HasComment("类型：SO销售/PR领料/OTHER其他")
                .HasColumnName("out_type");
            entity.Property(e => e.SourceNo)
                .HasMaxLength(50)
                .HasComment("来源单据号")
                .HasColumnName("source_no");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("0待审/1已审")
                .HasColumnName("status");
            entity.Property(e => e.WarehouseId)
                .HasComment("仓库ID")
                .HasColumnName("warehouse_id");
        });

        modelBuilder.Entity<InvStock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("inv_stock", tb => tb.HasComment("库存实时台账"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.WarehouseId, "idx_stock_wh");

            entity.HasIndex(e => new { e.MaterialId, e.WarehouseId }, "uk_stock_mat_wh").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CurrentQty)
                .HasPrecision(18, 4)
                .HasComment("当前库存数量")
                .HasColumnName("current_qty");
            entity.Property(e => e.LastUpdate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("last_update");
            entity.Property(e => e.LockedQty)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("锁定库存")
                .HasColumnName("locked_qty");
            entity.Property(e => e.MaterialId)
                .HasComment("物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.WarehouseId)
                .HasComment("仓库ID")
                .HasColumnName("warehouse_id");
        });

        modelBuilder.Entity<InvTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("inv_transaction", tb => tb.HasComment("库存流水"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => new { e.MaterialId, e.WarehouseId }, "idx_trans_mat_wh");

            entity.HasIndex(e => e.TransTime, "idx_trans_time");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AfterQty)
                .HasPrecision(18, 4)
                .HasComment("变动后数量")
                .HasColumnName("after_qty");
            entity.Property(e => e.BeforeQty)
                .HasPrecision(18, 4)
                .HasComment("变动前数量")
                .HasColumnName("before_qty");
            entity.Property(e => e.Direction)
                .HasComment("1入库/-1出库")
                .HasColumnName("direction");
            entity.Property(e => e.MaterialId)
                .HasComment("物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.Quantity)
                .HasPrecision(18, 4)
                .HasComment("变动数量（正数）")
                .HasColumnName("quantity");
            entity.Property(e => e.SourceNo)
                .HasMaxLength(50)
                .HasComment("来源单据号")
                .HasColumnName("source_no");
            entity.Property(e => e.TransNo)
                .HasMaxLength(50)
                .HasComment("流水单号")
                .HasColumnName("trans_no");
            entity.Property(e => e.TransTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("trans_time");
            entity.Property(e => e.TransType)
                .HasMaxLength(20)
                .HasComment("IN_PO/OUT_SO/IN_PR/OUT_PR/ADJ")
                .HasColumnName("trans_type");
            entity.Property(e => e.WarehouseId)
                .HasComment("仓库ID")
                .HasColumnName("warehouse_id");
        });

        modelBuilder.Entity<InvTransfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("inv_transfer", tb => tb.HasComment("调拨单"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.TransferNo, "uk_transfer_no").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AuditStatus)
                .HasDefaultValueSql("'0'")
                .HasComment("审批状态：0待审/1通过/2驳回")
                .HasColumnName("audit_status");
            entity.Property(e => e.FromWarehouseId)
                .HasComment("调出仓库ID")
                .HasColumnName("from_warehouse_id");
            entity.Property(e => e.MaterialId)
                .HasComment("物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.Quantity)
                .HasPrecision(18, 4)
                .HasComment("调拨数量")
                .HasColumnName("quantity");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("0待审/1已审")
                .HasColumnName("status");
            entity.Property(e => e.ToWarehouseId)
                .HasComment("调入仓库ID")
                .HasColumnName("to_warehouse_id");
            entity.Property(e => e.TransferDate)
                .HasComment("调拨日期")
                .HasColumnName("transfer_date");
            entity.Property(e => e.TransferNo)
                .HasMaxLength(50)
                .HasComment("调拨单号")
                .HasColumnName("transfer_no");
        });

        modelBuilder.Entity<MdAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("md_account", tb => tb.HasComment("会计科目"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.AccountCode, "uk_account_code").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("科目ID")
                .HasColumnName("id");
            entity.Property(e => e.AccountCode)
                .HasMaxLength(20)
                .HasComment("科目编码")
                .HasColumnName("account_code");
            entity.Property(e => e.AccountName)
                .HasMaxLength(100)
                .HasComment("科目名称")
                .HasColumnName("account_name");
            entity.Property(e => e.AccountType)
                .HasMaxLength(20)
                .HasComment("科目类型：资产/负债/权益/成本/损益")
                .HasColumnName("account_type");
            entity.Property(e => e.ParentId)
                .HasDefaultValueSql("'0'")
                .HasComment("上级科目ID")
                .HasColumnName("parent_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'1'")
                .HasComment("状态：1启用/0停用")
                .HasColumnName("status");
        });

        modelBuilder.Entity<MdBom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("md_bom", tb => tb.HasComment("BOM物料清单"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.ChildMaterialId, "idx_bom_child");

            entity.HasIndex(e => e.ParentMaterialId, "idx_bom_parent");

            entity.Property(e => e.Id)
                .HasComment("BOM ID")
                .HasColumnName("id");
            entity.Property(e => e.ChildMaterialId)
                .HasComment("子项物料ID（原材料/半成品）")
                .HasColumnName("child_material_id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.LossRate)
                .HasPrecision(6, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("损耗率（0.02=2%）")
                .HasColumnName("loss_rate");
            entity.Property(e => e.ParentMaterialId)
                .HasComment("父项物料ID（成品/半成品）")
                .HasColumnName("parent_material_id");
            entity.Property(e => e.QtyPerUnit)
                .HasPrecision(18, 6)
                .HasComment("单位用量")
                .HasColumnName("qty_per_unit");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'1'")
                .HasComment("状态：1启用/0停用")
                .HasColumnName("status");
            entity.Property(e => e.Version)
                .HasMaxLength(20)
                .HasDefaultValueSql("'V1.0'")
                .HasComment("BOM版本")
                .HasColumnName("version");
        });

        modelBuilder.Entity<MdCustomer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("md_customer", tb => tb.HasComment("客户档案"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.CustomerCode, "uk_customer_code").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("客户ID")
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(300)
                .HasComment("地址")
                .HasColumnName("address");
            entity.Property(e => e.ContactPerson)
                .HasMaxLength(50)
                .HasComment("联系人")
                .HasColumnName("contact_person");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.CustomerCode)
                .HasMaxLength(50)
                .HasComment("客户编码")
                .HasColumnName("customer_code");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(200)
                .HasComment("客户名称")
                .HasColumnName("customer_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .HasComment("联系电话")
                .HasColumnName("phone");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'1'")
                .HasComment("状态：1启用/0停用")
                .HasColumnName("status");
        });

        modelBuilder.Entity<MdMaterial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("md_material", tb => tb.HasComment("物料主数据"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.MaterialCode, "uk_material_code").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("物料ID")
                .HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasComment("物料分类：成品/半成品/原材料/辅料")
                .HasColumnName("category");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.MaterialCode)
                .HasMaxLength(50)
                .HasComment("物料编码")
                .HasColumnName("material_code");
            entity.Property(e => e.MaterialName)
                .HasMaxLength(200)
                .HasComment("物料名称")
                .HasColumnName("material_name");
            entity.Property(e => e.SafetyStock)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("安全库存")
                .HasColumnName("safety_stock");
            entity.Property(e => e.Spec)
                .HasMaxLength(200)
                .HasComment("规格型号")
                .HasColumnName("spec");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'1'")
                .HasComment("状态：1启用/0停用")
                .HasColumnName("status");
            entity.Property(e => e.Unit)
                .HasMaxLength(20)
                .HasComment("计量单位")
                .HasColumnName("unit");
        });

        modelBuilder.Entity<MdSupplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("md_supplier", tb => tb.HasComment("供应商档案"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.SupplierCode, "uk_supplier_code").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("供应商ID")
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(300)
                .HasComment("地址")
                .HasColumnName("address");
            entity.Property(e => e.ContactPerson)
                .HasMaxLength(50)
                .HasComment("联系人")
                .HasColumnName("contact_person");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .HasComment("联系电话")
                .HasColumnName("phone");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'1'")
                .HasComment("状态：1启用/0停用")
                .HasColumnName("status");
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(50)
                .HasComment("供应商编码")
                .HasColumnName("supplier_code");
            entity.Property(e => e.SupplierName)
                .HasMaxLength(200)
                .HasComment("供应商名称")
                .HasColumnName("supplier_name");
        });

        modelBuilder.Entity<MdWarehouse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("md_warehouse", tb => tb.HasComment("仓库档案"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.WarehouseCode, "uk_warehouse_code").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("仓库ID")
                .HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.Location)
                .HasMaxLength(200)
                .HasComment("仓库位置")
                .HasColumnName("location");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'1'")
                .HasComment("状态：1启用/0停用")
                .HasColumnName("status");
            entity.Property(e => e.WarehouseCode)
                .HasMaxLength(50)
                .HasComment("仓库编码")
                .HasColumnName("warehouse_code");
            entity.Property(e => e.WarehouseName)
                .HasMaxLength(100)
                .HasComment("仓库名称")
                .HasColumnName("warehouse_name");
        });

        modelBuilder.Entity<PoOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("po_order", tb => tb.HasComment("采购订单主表"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.SupplierId, "idx_po_supplier");

            entity.HasIndex(e => e.OrderNo, "uk_po_order_no").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.OrderDate)
                .HasComment("下单日期")
                .HasColumnName("order_date");
            entity.Property(e => e.OrderNo)
                .HasMaxLength(50)
                .HasComment("采购订单号")
                .HasColumnName("order_no");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("0待审/1已审/2部分收货/3已完成")
                .HasColumnName("status");
            entity.Property(e => e.AuditStatus)
                .HasDefaultValueSql("'0'")
                .HasComment("审批状态：0待审/1通过/2驳回（变更审批回写）")
                .HasColumnName("audit_status");
            entity.Property(e => e.SupplierId)
                .HasComment("供应商ID")
                .HasColumnName("supplier_id");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("'0.00'")
                .HasComment("订单总金额")
                .HasColumnName("total_amount");
        });

        modelBuilder.Entity<PoOrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("po_order_detail", tb => tb.HasComment("采购订单明细"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.MaterialId, "idx_pod_material");

            entity.HasIndex(e => e.OrderId, "idx_pod_order");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaterialId)
                .HasComment("物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.OrderId)
                .HasComment("采购订单ID")
                .HasColumnName("order_id");
            entity.Property(e => e.Quantity)
                .HasPrecision(18, 4)
                .HasComment("采购数量")
                .HasColumnName("quantity");
            entity.Property(e => e.ReceivedQty)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("已收货数量")
                .HasColumnName("received_qty");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(18, 6)
                .HasComment("含税单价")
                .HasColumnName("unit_price");
        });

        modelBuilder.Entity<PoReceipt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("po_receipt", tb => tb.HasComment("采购收货单"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.OrderId, "idx_pr_order");

            entity.HasIndex(e => e.ReceiptNo, "uk_po_receipt_no").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.OrderId)
                .HasComment("采购订单ID")
                .HasColumnName("order_id");
            entity.Property(e => e.ReceiptDate)
                .HasComment("收货日期")
                .HasColumnName("receipt_date");
            entity.Property(e => e.ReceiptNo)
                .HasMaxLength(50)
                .HasComment("收货单号")
                .HasColumnName("receipt_no");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("0待检/1合格/2不合格")
                .HasColumnName("status");
            entity.Property(e => e.AuditStatus)
                .HasDefaultValueSql("'0'")
                .HasComment("审批状态：0待审/1通过/2驳回")
                .HasColumnName("audit_status");
            entity.Property(e => e.SupplierId)
                .HasComment("供应商ID")
                .HasColumnName("supplier_id");
        });

        modelBuilder.Entity<PoReceiptDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("po_receipt_detail", tb => tb.HasComment("采购收货明细"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.ReceiptId, "idx_prd_receipt");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaterialId)
                .HasComment("物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.QualifiedQty)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("合格数量")
                .HasColumnName("qualified_qty");
            entity.Property(e => e.Quantity)
                .HasPrecision(18, 4)
                .HasComment("收货数量")
                .HasColumnName("quantity");
            entity.Property(e => e.ReceiptId)
                .HasComment("收货单ID")
                .HasColumnName("receipt_id");
        });

        modelBuilder.Entity<PoReturn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("po_return", tb => tb.HasComment("采购退货单"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.ReturnNo, "uk_po_return_no").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaterialId)
                .HasComment("物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.Quantity)
                .HasPrecision(18, 4)
                .HasComment("退货数量")
                .HasColumnName("quantity");
            entity.Property(e => e.ReceiptId)
                .HasComment("来源收货单ID")
                .HasColumnName("receipt_id");
            entity.Property(e => e.ReturnDate)
                .HasComment("退货日期")
                .HasColumnName("return_date");
            entity.Property(e => e.ReturnNo)
                .HasMaxLength(50)
                .HasComment("退货单号")
                .HasColumnName("return_no");
            entity.Property(e => e.SupplierId)
                .HasComment("供应商ID")
                .HasColumnName("supplier_id");
        });

        modelBuilder.Entity<PoSupplierQuote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("po_supplier_quote", tb => tb.HasComment("供应商报价"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.MaterialId, "idx_quote_material");

            entity.HasIndex(e => e.SupplierId, "idx_quote_supplier");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.MaterialId)
                .HasComment("物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.QuotePrice)
                .HasPrecision(18, 6)
                .HasComment("报价单价")
                .HasColumnName("quote_price");
            entity.Property(e => e.SupplierId)
                .HasComment("供应商ID")
                .HasColumnName("supplier_id");
            entity.Property(e => e.ValidFrom)
                .HasComment("有效期起")
                .HasColumnName("valid_from");
            entity.Property(e => e.ValidTo)
                .HasComment("有效期止")
                .HasColumnName("valid_to");
        });

        modelBuilder.Entity<PrMaterialIssue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("pr_material_issue", tb => tb.HasComment("生产领料"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.PrOrderId, "idx_issue_pr");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IssueQty)
                .HasPrecision(18, 4)
                .HasComment("领用数量")
                .HasColumnName("issue_qty");
            entity.Property(e => e.IssueTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("领用时间")
                .HasColumnType("datetime")
                .HasColumnName("issue_time");
            entity.Property(e => e.MaterialId)
                .HasComment("领用物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.PrOrderId)
                .HasComment("生产订单ID")
                .HasColumnName("pr_order_id");
        });

        modelBuilder.Entity<PrMaterialReturn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("pr_material_return", tb => tb.HasComment("生产退料"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.PrOrderId, "idx_return_pr");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaterialId)
                .HasComment("退料物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.PrOrderId)
                .HasComment("生产订单ID")
                .HasColumnName("pr_order_id");
            entity.Property(e => e.ReturnQty)
                .HasPrecision(18, 4)
                .HasComment("退料数量")
                .HasColumnName("return_qty");
            entity.Property(e => e.ReturnTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("退料时间")
                .HasColumnType("datetime")
                .HasColumnName("return_time");
        });

        modelBuilder.Entity<PrMp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("pr_mps", tb => tb.HasComment("主生产计划MPS"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.MaterialId, "idx_mps_material");

            entity.HasIndex(e => e.MpsNo, "uk_mps_no").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.MaterialId)
                .HasComment("成品物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.MpsNo)
                .HasMaxLength(50)
                .HasComment("MPS计划号")
                .HasColumnName("mps_no");
            entity.Property(e => e.PlanEndDate)
                .HasComment("计划完工日")
                .HasColumnName("plan_end_date");
            entity.Property(e => e.PlanQty)
                .HasPrecision(18, 4)
                .HasComment("计划数量")
                .HasColumnName("plan_qty");
            entity.Property(e => e.PlanStartDate)
                .HasComment("计划开工日")
                .HasColumnName("plan_start_date");
            entity.Property(e => e.SourceId)
                .HasComment("来源单据ID")
                .HasColumnName("source_id");
            entity.Property(e => e.SourceType)
                .HasMaxLength(20)
                .HasComment("来源：SO销售/FCST预测/STOCK安全库存")
                .HasColumnName("source_type");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("0草稿/1确认/2已转生产订单")
                .HasColumnName("status");
        });

        modelBuilder.Entity<PrMrp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("pr_mrp", tb => tb.HasComment("MRP运算结果"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.MaterialId, "idx_mrp_material");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.GrossRequirement)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("毛需求")
                .HasColumnName("gross_requirement");
            entity.Property(e => e.MaterialId)
                .HasComment("物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.MrpNo)
                .HasMaxLength(50)
                .HasComment("MRP运算号")
                .HasColumnName("mrp_no");
            entity.Property(e => e.NetRequirement)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("净需求")
                .HasColumnName("net_requirement");
            entity.Property(e => e.OnHandQty)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("现有库存")
                .HasColumnName("on_hand_qty");
            entity.Property(e => e.OnOrderQty)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("在途量")
                .HasColumnName("on_order_qty");
            entity.Property(e => e.SafetyStock)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("安全库存")
                .HasColumnName("safety_stock");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("0未处理/1已转单")
                .HasColumnName("status");
            entity.Property(e => e.SuggestDate)
                .HasComment("建议日期")
                .HasColumnName("suggest_date");
            entity.Property(e => e.SuggestQty)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("建议数量")
                .HasColumnName("suggest_qty");
            entity.Property(e => e.SuggestType)
                .HasMaxLength(20)
                .HasComment("建议类型：BUY采购/MAKE自制")
                .HasColumnName("suggest_type");
        });

        modelBuilder.Entity<PrOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("pr_order", tb => tb.HasComment("生产订单"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.MaterialId, "idx_pr_mat");

            entity.HasIndex(e => e.OrderNo, "uk_pr_order_no").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompletedQty)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("已完工入库数量")
                .HasColumnName("completed_qty");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.EndDate)
                .HasComment("计划完工日")
                .HasColumnName("end_date");
            entity.Property(e => e.MaterialId)
                .HasComment("要生产的成品ID")
                .HasColumnName("material_id");
            entity.Property(e => e.OrderNo)
                .HasMaxLength(50)
                .HasComment("生产制令号")
                .HasColumnName("order_no");
            entity.Property(e => e.PlanQty)
                .HasPrecision(18, 4)
                .HasComment("计划生产数量")
                .HasColumnName("plan_qty");
            entity.Property(e => e.SourceMpsId)
                .HasComment("来源MPS ID")
                .HasColumnName("source_mps_id");
            entity.Property(e => e.SourceSoId)
                .HasComment("来源销售订单ID")
                .HasColumnName("source_so_id");
            entity.Property(e => e.StartDate)
                .HasComment("计划开工日")
                .HasColumnName("start_date");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("0计划/1下达/2生产中/3完工/4关闭")
                .HasColumnName("status");
        });

        modelBuilder.Entity<PrOutsourcing>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("pr_outsourcing", tb => tb.HasComment("委外订单"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.OutsourceNo, "uk_outsource_no").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndDate)
                .HasComment("结束日期")
                .HasColumnName("end_date");
            entity.Property(e => e.MaterialId)
                .HasComment("委外成品ID")
                .HasColumnName("material_id");
            entity.Property(e => e.OutsourceNo)
                .HasMaxLength(50)
                .HasComment("委外订单号")
                .HasColumnName("outsource_no");
            entity.Property(e => e.PlanQty)
                .HasPrecision(18, 4)
                .HasComment("委外数量")
                .HasColumnName("plan_qty");
            entity.Property(e => e.ReceivedQty)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("已回收数量")
                .HasColumnName("received_qty");
            entity.Property(e => e.StartDate)
                .HasComment("开始日期")
                .HasColumnName("start_date");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("0计划/1外发/2部分回收/3完成")
                .HasColumnName("status");
            entity.Property(e => e.SupplierId)
                .HasComment("加工商ID")
                .HasColumnName("supplier_id");
        });

        modelBuilder.Entity<PrOutsourcingIssue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("pr_outsourcing_issue", tb => tb.HasComment("委外发料"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.OutsourceId, "idx_os_issue");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IssueQty)
                .HasPrecision(18, 4)
                .HasComment("发料数量")
                .HasColumnName("issue_qty");
            entity.Property(e => e.IssueTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("issue_time");
            entity.Property(e => e.MaterialId)
                .HasComment("发料物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.OutsourceId)
                .HasComment("委外订单ID")
                .HasColumnName("outsource_id");
        });

        modelBuilder.Entity<PrReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("pr_report", tb => tb.HasComment("生产报工"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.PrOrderId, "idx_report_pr");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BadQty)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("不良品数量")
                .HasColumnName("bad_qty");
            entity.Property(e => e.GoodQty)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("良品数量")
                .HasColumnName("good_qty");
            entity.Property(e => e.PrOrderId)
                .HasComment("生产订单ID")
                .HasColumnName("pr_order_id");
            entity.Property(e => e.ProcessName)
                .HasMaxLength(100)
                .HasComment("工序名称")
                .HasColumnName("process_name");
            entity.Property(e => e.ReportQty)
                .HasPrecision(18, 4)
                .HasComment("报工数量")
                .HasColumnName("report_qty");
            entity.Property(e => e.ReportTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("报工时间")
                .HasColumnType("datetime")
                .HasColumnName("report_time");
            entity.Property(e => e.WorkHours)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasComment("工时")
                .HasColumnName("work_hours");
        });

        modelBuilder.Entity<RptKpi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("rpt_kpi", tb => tb.HasComment("看板指标"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => new { e.KpiCode, e.CalcDate }, "uk_kpi_code_date").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CalcDate)
                .HasComment("统计日期")
                .HasColumnName("calc_date");
            entity.Property(e => e.KpiCode)
                .HasMaxLength(50)
                .HasComment("指标编码")
                .HasColumnName("kpi_code");
            entity.Property(e => e.KpiName)
                .HasMaxLength(100)
                .HasComment("指标名称")
                .HasColumnName("kpi_name");
            entity.Property(e => e.KpiUnit)
                .HasMaxLength(20)
                .HasComment("单位")
                .HasColumnName("kpi_unit");
            entity.Property(e => e.KpiValue)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("指标值")
                .HasColumnName("kpi_value");
            entity.Property(e => e.UpdateTime)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("update_time");
        });

        modelBuilder.Entity<RptReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("rpt_report", tb => tb.HasComment("报表定义"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.ReportCode, "uk_report_code").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.ReportCode)
                .HasMaxLength(50)
                .HasComment("报表编码")
                .HasColumnName("report_code");
            entity.Property(e => e.ReportName)
                .HasMaxLength(200)
                .HasComment("报表名称")
                .HasColumnName("report_name");
            entity.Property(e => e.ReportType)
                .HasMaxLength(20)
                .HasComment("类型：TABLE/CHART")
                .HasColumnName("report_type");
            entity.Property(e => e.SqlText)
                .HasComment("取数SQL")
                .HasColumnType("text")
                .HasColumnName("sql_text");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'1'")
                .HasComment("1启用/0停用")
                .HasColumnName("status");
        });

        modelBuilder.Entity<SoCustomerCredit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("so_customer_credit", tb => tb.HasComment("客户信用"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.CustomerId, "uk_credit_customer").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreditLimit)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("'0.00'")
                .HasComment("信用额度")
                .HasColumnName("credit_limit");
            entity.Property(e => e.CustomerId)
                .HasComment("客户ID")
                .HasColumnName("customer_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'1'")
                .HasComment("1正常/0冻结")
                .HasColumnName("status");
            entity.Property(e => e.AuditStatus)
                .HasDefaultValueSql("'0'")
                .HasComment("审批状态：0待审/1通过/2驳回")
                .HasColumnName("audit_status");
            entity.Property(e => e.UpdateTime)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("update_time");
            entity.Property(e => e.UsedAmount)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("'0.00'")
                .HasComment("已用额度")
                .HasColumnName("used_amount");
        });

        modelBuilder.Entity<SoDelivery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("so_delivery", tb => tb.HasComment("发货单"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.OrderId, "idx_delivery_order");

            entity.HasIndex(e => e.DeliveryNo, "uk_delivery_no").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.CustomerId)
                .HasComment("客户ID")
                .HasColumnName("customer_id");
            entity.Property(e => e.DeliveryDate)
                .HasComment("发货日期")
                .HasColumnName("delivery_date");
            entity.Property(e => e.DeliveryNo)
                .HasMaxLength(50)
                .HasComment("发货单号")
                .HasColumnName("delivery_no");
            entity.Property(e => e.OrderId)
                .HasComment("销售订单ID")
                .HasColumnName("order_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("0待发/1已发/2已签收")
                .HasColumnName("status");
        });

        modelBuilder.Entity<SoDeliveryDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("so_delivery_detail", tb => tb.HasComment("发货明细"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.DeliveryId, "idx_dd_delivery");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DeliveryId)
                .HasComment("发货单ID")
                .HasColumnName("delivery_id");
            entity.Property(e => e.MaterialId)
                .HasComment("物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.Quantity)
                .HasPrecision(18, 4)
                .HasComment("发货数量")
                .HasColumnName("quantity");
        });

        modelBuilder.Entity<SoOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("so_order", tb => tb.HasComment("销售订单主表"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.CustomerId, "idx_so_customer");

            entity.HasIndex(e => e.OrderNo, "uk_so_order_no").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId)
                .HasComment("客户ID")
                .HasColumnName("customer_id");
            entity.Property(e => e.DeliveryDate)
                .HasComment("要求交货日期")
                .HasColumnName("delivery_date");
            entity.Property(e => e.OrderDate)
                .HasComment("订单日期")
                .HasColumnName("order_date");
            entity.Property(e => e.OrderNo)
                .HasMaxLength(50)
                .HasComment("销售订单号")
                .HasColumnName("order_no");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("0新建/1已确认/2已发货/3已完成")
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("total_amount");
        });

        modelBuilder.Entity<SoOrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("so_order_detail", tb => tb.HasComment("销售订单明细"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.MaterialId, "idx_sod_material");

            entity.HasIndex(e => e.OrderId, "idx_sod_order");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaterialId)
                .HasComment("物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.OrderId)
                .HasComment("销售订单ID")
                .HasColumnName("order_id");
            entity.Property(e => e.Quantity)
                .HasPrecision(18, 4)
                .HasComment("销售数量")
                .HasColumnName("quantity");
            entity.Property(e => e.ShippedQty)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("已发货数量")
                .HasColumnName("shipped_qty");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(18, 6)
                .HasComment("单价")
                .HasColumnName("unit_price");
        });

        modelBuilder.Entity<SoReturn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("so_return", tb => tb.HasComment("销售退货单"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.ReturnNo, "uk_so_return_no").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CustomerId)
                .HasComment("客户ID")
                .HasColumnName("customer_id");
            entity.Property(e => e.DeliveryId)
                .HasComment("来源发货单ID")
                .HasColumnName("delivery_id");
            entity.Property(e => e.MaterialId)
                .HasComment("物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.Quantity)
                .HasPrecision(18, 4)
                .HasComment("退货数量")
                .HasColumnName("quantity");
            entity.Property(e => e.ReturnDate)
                .HasComment("退货日期")
                .HasColumnName("return_date");
            entity.Property(e => e.ReturnNo)
                .HasMaxLength(50)
                .HasComment("退货单号")
                .HasColumnName("return_no");
        });

        modelBuilder.Entity<SysDeptRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("sys_dept_role", tb => tb.HasComment("部门角色关联（RBAC）"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => new { e.DepartmentId, e.RoleId }, "idx_dept_role");

            entity.Property(e => e.Id)
                .HasMaxLength(32)
                .HasComment("主键（GUID，NewSequentialGuid）")
                .HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("创建时间")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.DepartmentId)
                .HasComment("部门ID")
                .HasColumnName("department_id");
            entity.Property(e => e.IsBan)
                .HasComment("是否禁用：1禁用/0正常")
                .HasColumnName("is_ban");
            entity.Property(e => e.RoleId)
                .HasComment("角色ID（sys_role.id）")
                .HasColumnName("role_id");
        });

        modelBuilder.Entity<SysDeptRoleMenu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("sys_dept_role_menu", tb => tb.HasComment("部门角色菜单关联（RBAC）"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.DeptRoleId, "idx_dept_role_id");

            entity.HasIndex(e => e.MenuId, "idx_menu_id");

            entity.Property(e => e.Id)
                .HasComment("主键")
                .HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("创建时间")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.DeptRoleId)
                .HasMaxLength(32)
                .HasComment("部门角色ID（sys_dept_role.id）")
                .HasColumnName("dept_role_id");
            entity.Property(e => e.MenuId)
                .HasMaxLength(32)
                .HasComment("菜单ID（sys_menu.id）")
                .HasColumnName("menu_id");
        });

        modelBuilder.Entity<SysMenu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("sys_menu", tb => tb.HasComment("系统菜单（RBAC）"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.Path, "idx_path");

            entity.Property(e => e.Id)
                .HasMaxLength(32)
                .HasComment("主键（GUID，NewSequentialGuid）")
                .HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("创建时间")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.Icon)
                .HasMaxLength(100)
                .HasComment("图标")
                .HasColumnName("icon");
            entity.Property(e => e.IsBan)
                .HasComment("是否禁用：1禁用/0正常")
                .HasColumnName("is_ban");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasComment("菜单名称")
                .HasColumnName("name");
            entity.Property(e => e.Path)
                .HasMaxLength(200)
                .HasComment("路由/权限路径（如 /sys/menu，对应 Permission_Attribute 的 key）")
                .HasColumnName("path");
            entity.Property(e => e.Sort)
                .HasComment("排序")
                .HasColumnName("sort");
            entity.Property(e => e.Type)
                .HasDefaultValueSql("'2'")
                .HasComment("类型：1目录/2菜单/3按钮")
                .HasColumnName("type");
        });

        modelBuilder.Entity<SysOperLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("sys_oper_log", tb => tb.HasComment("操作日志"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.OperTime, "idx_oper_time");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .HasComment("操作类型")
                .HasColumnName("action");
            entity.Property(e => e.Detail)
                .HasMaxLength(500)
                .HasComment("操作详情")
                .HasColumnName("detail");
            entity.Property(e => e.Module)
                .HasMaxLength(50)
                .HasComment("模块")
                .HasColumnName("module");
            entity.Property(e => e.OperTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("oper_time");
            entity.Property(e => e.UserId)
                .HasComment("操作人ID")
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<SysRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("sys_role", tb => tb.HasComment("系统角色"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.RoleCode, "uk_role_code").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("角色ID")
                .HasColumnName("id");
            entity.Property(e => e.RoleCode)
                .HasMaxLength(50)
                .HasComment("角色编码")
                .HasColumnName("role_code");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .HasComment("角色名称")
                .HasColumnName("role_name");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'1'")
                .HasComment("状态：1启用/0停用")
                .HasColumnName("status");
        });

        modelBuilder.Entity<SysUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("sys_user", tb => tb.HasComment("系统用户"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.Username, "uk_username").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("用户ID")
                .HasColumnName("id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .HasComment("密码（加密存储）")
                .HasColumnName("password");
            entity.Property(e => e.RealName)
                .HasMaxLength(50)
                .HasComment("真实姓名")
                .HasColumnName("real_name");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'1'")
                .HasComment("状态：1启用/0停用")
                .HasColumnName("status");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasComment("登录名")
                .HasColumnName("username");
        });

        modelBuilder.Entity<SysUserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("sys_user_role", tb => tb.HasComment("用户角色关联"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => new { e.UserId, e.RoleId }, "uk_user_role").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RoleId)
                .HasComment("角色ID")
                .HasColumnName("role_id");
            entity.Property(e => e.UserId)
                .HasComment("用户ID")
                .HasColumnName("user_id");
        });

        // ================= 扩展表：与主表同款 Fluent API 集中配置 =================
        // 表结构仍由 SqlScript/erp_db*.sql 创建，此处仅为 EF 模型与查询服务。

        modelBuilder.Entity<SysExLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("sys_ex_log", tb => tb.HasComment("异常日志"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RequestPath)
                .HasMaxLength(200)
                .HasComment("请求路径")
                .HasColumnName("request_path");
            entity.Property(e => e.Message)
                .HasMaxLength(2000)
                .HasComment("异常消息")
                .HasColumnName("message");
            entity.Property(e => e.StackTrace)
                .HasColumnType("text")
                .HasComment("堆栈信息")
                .HasColumnName("stack_trace");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
        });

        modelBuilder.Entity<PayNotify>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("pay_notify", tb => tb.HasComment("支付回调通知"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.NotifyNo, "uk_notify_no").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NotifyNo)
                .HasMaxLength(64)
                .HasComment("通知流水号（幂等键）")
                .HasColumnName("notify_no");
            entity.Property(e => e.Channel)
                .HasMaxLength(20)
                .HasComment("支付渠道")
                .HasColumnName("channel");
            entity.Property(e => e.BizNo)
                .HasMaxLength(50)
                .HasComment("业务单号")
                .HasColumnName("biz_no");
            entity.Property(e => e.ReceivableId)
                .HasComment("应收单ID")
                .HasColumnName("receivable_id");
            entity.Property(e => e.Amount)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("'0.00'")
                .HasComment("通知金额")
                .HasColumnName("amount");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("状态：0待处理/1已处理")
                .HasColumnName("status");
            entity.Property(e => e.Payload)
                .HasColumnType("text")
                .HasComment("原始报文")
                .HasColumnName("payload");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("update_time");
        });

        modelBuilder.Entity<SysApproval>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("sys_approval", tb => tb.HasComment("审批单"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.ApprovalNo, "uk_approval_no").IsUnique();

            entity.HasIndex(e => e.Status, "idx_appr_status");

            entity.HasIndex(e => new { e.BizType, e.BizId }, "idx_appr_biz");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApprovalNo)
                .HasMaxLength(50)
                .HasComment("审批单号")
                .HasColumnName("approval_no");
            entity.Property(e => e.BizType)
                .HasMaxLength(30)
                .HasComment("单据类型：SO/PO/PR/VOUCHER/PO_REQ/OTHER")
                .HasColumnName("biz_type");
            entity.Property(e => e.BizId)
                .HasComment("业务单据ID")
                .HasColumnName("biz_id");
            entity.Property(e => e.BillNo)
                .HasMaxLength(50)
                .HasComment("业务单据号（冗余，便于查看）")
                .HasColumnName("bill_no");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasComment("审批标题")
                .HasColumnName("title");
            entity.Property(e => e.Amount)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("'0.00'")
                .HasComment("金额")
                .HasColumnName("amount");
            entity.Property(e => e.ApplicantId)
                .HasComment("申请人ID")
                .HasColumnName("applicant_id");
            entity.Property(e => e.ApplicantName)
                .HasMaxLength(50)
                .HasComment("申请人姓名")
                .HasColumnName("applicant_name");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("状态：0草稿/1审批中/2已通过/3已驳回")
                .HasColumnName("status");
            entity.Property(e => e.CurrentNode)
                .HasDefaultValueSql("'0'")
                .HasComment("当前审批节点序号")
                .HasColumnName("current_node");
            entity.Property(e => e.TotalNodes)
                .HasDefaultValueSql("'1'")
                .HasComment("审批节点总数")
                .HasColumnName("total_nodes");
            entity.Property(e => e.NodeRoles)
                .HasMaxLength(200)
                .HasComment("各节点审批角色序列，逗号分隔；第n个值对应第n个节点")
                .HasColumnName("node_roles");
            entity.Property(e => e.Remark)
                .HasMaxLength(500)
                .HasComment("备注")
                .HasColumnName("remark");
            entity.Property(e => e.SubmitTime)
                .HasColumnType("datetime")
                .HasColumnName("submit_time");
            entity.Property(e => e.FinishTime)
                .HasColumnType("datetime")
                .HasColumnName("finish_time");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
        });

        modelBuilder.Entity<SysApprovalLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("sys_approval_log", tb => tb.HasComment("审批记录"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.ApprovalId, "idx_alog_approval");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApprovalId)
                .HasComment("审批单ID")
                .HasColumnName("approval_id");
            entity.Property(e => e.NodeNo)
                .HasDefaultValueSql("'0'")
                .HasComment("节点序号，0=提交")
                .HasColumnName("node_no");
            entity.Property(e => e.Action)
                .HasComment("动作：1提交/2通过/3驳回/4撤回")
                .HasColumnName("action");
            entity.Property(e => e.ApproverId)
                .HasComment("审批人ID")
                .HasColumnName("approver_id");
            entity.Property(e => e.ApproverName)
                .HasMaxLength(50)
                .HasComment("审批人姓名")
                .HasColumnName("approver_name");
            entity.Property(e => e.Opinion)
                .HasMaxLength(500)
                .HasComment("审批意见")
                .HasColumnName("opinion");
            entity.Property(e => e.OperTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasComment("操作时间")
                .HasColumnName("oper_time");
        });

        modelBuilder.Entity<SysApprovalRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("sys_approval_rule", tb => tb.HasComment("审批规则"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.BizType, "idx_rule_biz");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BizType)
                .HasMaxLength(30)
                .HasComment("业务类型：PO/SO/PR/QUOTE/VOUCHER/PAYMENT/INV/MD/OTHER")
                .HasColumnName("biz_type");
            entity.Property(e => e.MinAmount)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("'0.00'")
                .HasComment("金额下限（含）")
                .HasColumnName("min_amount");
            entity.Property(e => e.MaxAmount)
                .HasPrecision(18, 2)
                .HasComment("金额上限（不含），null 表示无上限")
                .HasColumnName("max_amount");
            entity.Property(e => e.Level)
                .HasDefaultValueSql("'1'")
                .HasComment("审批节点级数")
                .HasColumnName("level");
            entity.Property(e => e.NodeRoles)
                .HasMaxLength(200)
                .HasDefaultValueSql("'admin'")
                .HasComment("各节点审批角色序列，逗号分隔；第n个值对应第n个节点")
                .HasColumnName("node_roles");
            entity.Property(e => e.Enabled)
                .HasDefaultValueSql("'1'")
                .HasComment("是否启用：1启用/0停用")
                .HasColumnName("enabled");
            entity.Property(e => e.Remark)
                .HasMaxLength(200)
                .HasComment("备注")
                .HasColumnName("remark");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
        });

        modelBuilder.Entity<PoRequisition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("po_requisition", tb => tb.HasComment("请购单"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.ReqNo, "uk_req_no").IsUnique();

            entity.HasIndex(e => e.Status, "idx_req_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ReqNo)
                .HasMaxLength(50)
                .HasComment("请购单号")
                .HasColumnName("req_no");
            entity.Property(e => e.ReqDate)
                .HasColumnType("date")
                .HasComment("请购日期")
                .HasColumnName("req_date");
            entity.Property(e => e.DeptName)
                .HasMaxLength(100)
                .HasComment("申请部门")
                .HasColumnName("dept_name");
            entity.Property(e => e.ApplicantId)
                .HasComment("申请人ID")
                .HasColumnName("applicant_id");
            entity.Property(e => e.ApplicantName)
                .HasMaxLength(50)
                .HasComment("申请人姓名")
                .HasColumnName("applicant_name");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("状态：0草稿/1已转采购订单/2已驳回")
                .HasColumnName("status");
            entity.Property(e => e.AuditStatus)
                .HasDefaultValueSql("'0'")
                .HasComment("审批状态：0待审/1通过/2驳回")
                .HasColumnName("audit_status");
            entity.Property(e => e.PoOrderId)
                .HasComment("转生成的采购订单ID")
                .HasColumnName("po_order_id");
            entity.Property(e => e.Remark)
                .HasMaxLength(500)
                .HasComment("备注")
                .HasColumnName("remark");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
        });

        modelBuilder.Entity<PoRequisitionDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("po_requisition_detail", tb => tb.HasComment("请购单明细"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.ReqId, "idx_reqd_req");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ReqId)
                .HasComment("请购单ID")
                .HasColumnName("req_id");
            entity.Property(e => e.MaterialId)
                .HasComment("物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.ReqQty)
                .HasPrecision(18, 4)
                .HasComment("请购数量")
                .HasColumnName("req_qty");
            entity.Property(e => e.ConvertedQty)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasComment("已转订单数量")
                .HasColumnName("converted_qty");
            entity.Property(e => e.SuggestSupplierId)
                .HasComment("建议供应商ID")
                .HasColumnName("suggest_supplier_id");
            entity.Property(e => e.SuggestPrice)
                .HasPrecision(18, 6)
                .HasDefaultValueSql("'0.000000'")
                .HasComment("建议单价（取报价最优价）")
                .HasColumnName("suggest_price");
            entity.Property(e => e.Remark)
                .HasMaxLength(200)
                .HasComment("备注")
                .HasColumnName("remark");
        });

        modelBuilder.Entity<SoQuote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("so_quote", tb => tb.HasComment("销售报价单"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.QuoteNo, "uk_quote_no").IsUnique();

            entity.HasIndex(e => e.CustomerId, "idx_quote_customer");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.QuoteNo)
                .HasMaxLength(50)
                .HasComment("报价单号")
                .HasColumnName("quote_no");
            entity.Property(e => e.CustomerId)
                .HasComment("客户ID")
                .HasColumnName("customer_id");
            entity.Property(e => e.QuoteDate)
                .HasColumnType("date")
                .HasComment("报价日期")
                .HasColumnName("quote_date");
            entity.Property(e => e.ValidDate)
                .HasColumnType("date")
                .HasColumnName("valid_date");
            entity.Property(e => e.PriceStrategy)
                .HasMaxLength(20)
                .HasDefaultValueSql("'STANDARD'")
                .HasComment("价格策略：STANDARD标准/PREFER最低价/VIP折扣/CUSTOM协议价")
                .HasColumnName("price_strategy");
            entity.Property(e => e.DiscountRate)
                .HasPrecision(6, 4)
                .HasDefaultValueSql("'1.0000'")
                .HasComment("折扣率（1=不打折）")
                .HasColumnName("discount_rate");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("'0.00'")
                .HasComment("报价总额")
                .HasColumnName("total_amount");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("状态：0草稿/1已生效/2已转订单/3已失效")
                .HasColumnName("status");
            entity.Property(e => e.AuditStatus)
                .HasDefaultValueSql("'0'")
                .HasComment("审批状态：0待审/1通过/2驳回")
                .HasColumnName("audit_status");
            entity.Property(e => e.OrderId)
                .HasComment("转生成的销售订单ID")
                .HasColumnName("order_id");
            entity.Property(e => e.Remark)
                .HasMaxLength(500)
                .HasComment("备注")
                .HasColumnName("remark");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
        });

        modelBuilder.Entity<SoQuoteDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("so_quote_detail", tb => tb.HasComment("销售报价单明细"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.QuoteId, "idx_qd_quote");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.QuoteId)
                .HasComment("报价单ID")
                .HasColumnName("quote_id");
            entity.Property(e => e.MaterialId)
                .HasComment("物料ID")
                .HasColumnName("material_id");
            entity.Property(e => e.Quantity)
                .HasPrecision(18, 4)
                .HasComment("数量")
                .HasColumnName("quantity");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(18, 6)
                .HasComment("单价")
                .HasColumnName("unit_price");
            entity.Property(e => e.DiscountRate)
                .HasPrecision(6, 4)
                .HasDefaultValueSql("'1.0000'")
                .HasComment("行折扣率")
                .HasColumnName("discount_rate");
            entity.Property(e => e.Amount)
                .HasPrecision(18, 2)
                .HasDefaultValueSql("'0.00'")
                .HasComment("行金额")
                .HasColumnName("amount");
        });

        modelBuilder.Entity<SoDeliveryTrack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("so_delivery_track", tb => tb.HasComment("发货物流跟踪"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.DeliveryId, "idx_track_delivery");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DeliveryId)
                .HasComment("发货单ID")
                .HasColumnName("delivery_id");
            entity.Property(e => e.Carrier)
                .HasMaxLength(50)
                .HasComment("承运商")
                .HasColumnName("carrier");
            entity.Property(e => e.TrackNo)
                .HasMaxLength(50)
                .HasComment("运单号")
                .HasColumnName("track_no");
            entity.Property(e => e.TrackStatus)
                .HasMaxLength(30)
                .HasComment("状态：已揽收/运输中/派送中/已签收/异常")
                .HasColumnName("track_status");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasComment("当前位置")
                .HasColumnName("location");
            entity.Property(e => e.Remark)
                .HasMaxLength(300)
                .HasComment("备注")
                .HasColumnName("remark");
            entity.Property(e => e.TrackTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasComment("跟踪时间")
                .HasColumnName("track_time");
        });

        modelBuilder.Entity<MdLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("md_location", tb => tb.HasComment("库位档案"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => e.LocationCode, "uk_location_code").IsUnique();

            entity.HasIndex(e => e.WarehouseId, "idx_loc_warehouse");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.WarehouseId)
                .HasComment("所属仓库ID")
                .HasColumnName("warehouse_id");
            entity.Property(e => e.LocationCode)
                .HasMaxLength(50)
                .HasComment("库位编码")
                .HasColumnName("location_code");
            entity.Property(e => e.LocationName)
                .HasMaxLength(100)
                .HasComment("库位名称")
                .HasColumnName("location_name");
            entity.Property(e => e.Area)
                .HasMaxLength(50)
                .HasComment("区域")
                .HasColumnName("area");
            entity.Property(e => e.Shelf)
                .HasMaxLength(50)
                .HasComment("货架")
                .HasColumnName("shelf");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'1'")
                .HasComment("状态：1启用/0停用")
                .HasColumnName("status");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
        });

        modelBuilder.Entity<SysBillChange>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("sys_bill_change", tb => tb.HasComment("单据变更记录"))
                .UseCollation("utf8mb4_0900_ai_ci");

            entity.HasIndex(e => new { e.BillType, e.BillId }, "idx_change_bill");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BillType)
                .HasMaxLength(30)
                .HasComment("单据类型：SO/PO")
                .HasColumnName("bill_type");
            entity.Property(e => e.BillId)
                .HasComment("单据ID")
                .HasColumnName("bill_id");
            entity.Property(e => e.BillNo)
                .HasMaxLength(50)
                .HasComment("单据号")
                .HasColumnName("bill_no");
            entity.Property(e => e.ChangeType)
                .HasMaxLength(30)
                .HasComment("变更类型：QTY数量/PRICE价格/DATE交期/ADD新增行/DEL删除行")
                .HasColumnName("change_type");
            entity.Property(e => e.BeforeValue)
                .HasMaxLength(500)
                .HasComment("变更前")
                .HasColumnName("before_value");
            entity.Property(e => e.AfterValue)
                .HasMaxLength(500)
                .HasComment("变更后")
                .HasColumnName("after_value");
            entity.Property(e => e.Reason)
                .HasMaxLength(300)
                .HasComment("变更原因")
                .HasColumnName("reason");
            entity.Property(e => e.OperUser)
                .HasMaxLength(50)
                .HasComment("操作人")
                .HasColumnName("oper_user");
            entity.Property(e => e.OperTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasComment("操作时间")
                .HasColumnName("oper_time");
        });

        modelBuilder.Entity<HrDepartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_department", tb => tb.HasComment("部门（组织层级）")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DeptNo).HasColumnName("dept_no");
            entity.Property(e => e.DeptName).HasColumnName("dept_name");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.DeptLevel).HasColumnName("dept_level");
            entity.Property(e => e.ManagerId).HasColumnName("manager_id");
            entity.Property(e => e.ManagerName).HasMaxLength(50).HasComment("负责人姓名（手动填写）").HasColumnName("manager_name");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.OrderNo).HasColumnName("order_no");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrPosition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_position", tb => tb.HasComment("岗位体系")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PosNo).HasColumnName("pos_no");
            entity.Property(e => e.PosName).HasColumnName("pos_name");
            entity.Property(e => e.DeptId).HasColumnName("dept_id");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.PosLevel).HasColumnName("pos_level");
            entity.Property(e => e.JobFamily).HasColumnName("job_family");
            entity.Property(e => e.Responsibility).HasColumnName("responsibility");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrPositionQuota>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_position_quota", tb => tb.HasComment("岗位编制控制")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DeptId).HasColumnName("dept_id");
            entity.Property(e => e.PosId).HasColumnName("pos_id");
            entity.Property(e => e.PlanCount).HasColumnName("plan_count");
            entity.Property(e => e.ActualCount).HasColumnName("actual_count");
            entity.Property(e => e.FrozenCount).HasColumnName("frozen_count");
            entity.Property(e => e.Remark).HasColumnName("remark");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_employee", tb => tb.HasComment("员工主档")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmpNo).HasColumnName("emp_no");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.IdCard).HasColumnName("id_card");
            entity.Property(e => e.Nation).HasColumnName("nation");
            entity.Property(e => e.Marital).HasColumnName("marital");
            entity.Property(e => e.Education).HasColumnName("education");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.DeptId).HasColumnName("dept_id");
            entity.Property(e => e.PosId).HasColumnName("pos_id");
            entity.Property(e => e.EmpStatus).HasColumnName("emp_status");
            entity.Property(e => e.HireDate).HasColumnName("hire_date");
            entity.Property(e => e.RegularDate).HasColumnName("regular_date");
            entity.Property(e => e.LeaveDate).HasColumnName("leave_date");
            entity.Property(e => e.ContractCount).HasColumnName("contract_count");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrEmployeeChange>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_employee_change", tb => tb.HasComment("人事异动轨迹（全周期）")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.ChangeType).HasColumnName("change_type");
            entity.Property(e => e.OldDeptId).HasColumnName("old_dept_id");
            entity.Property(e => e.NewDeptId).HasColumnName("new_dept_id");
            entity.Property(e => e.OldPosId).HasColumnName("old_pos_id");
            entity.Property(e => e.NewPosId).HasColumnName("new_pos_id");
            entity.Property(e => e.OldStatus).HasColumnName("old_status");
            entity.Property(e => e.NewStatus).HasColumnName("new_status");
            entity.Property(e => e.EffectDate).HasColumnName("effect_date");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.ApprovalId).HasColumnName("approval_id");
            entity.Property(e => e.Operator).HasColumnName("operator");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrContract>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_contract", tb => tb.HasComment("劳动合同（含到期预警：按 end_date 查询）")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.ContractNo).HasColumnName("contract_no");
            entity.Property(e => e.ContractType).HasColumnName("contract_type");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.SignDate).HasColumnName("sign_date");
            entity.Property(e => e.ProbationMonths).HasColumnName("probation_months");
            entity.Property(e => e.Salary).HasColumnName("salary");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Remark).HasColumnName("remark");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrShift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_shift", tb => tb.HasComment("班次定义")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ShiftNo).HasColumnName("shift_no");
            entity.Property(e => e.ShiftName).HasColumnName("shift_name");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.CrossDay).HasColumnName("cross_day");
            entity.Property(e => e.RestMinutes).HasColumnName("rest_minutes");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_schedule", tb => tb.HasComment("排班表")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.ShiftId).HasColumnName("shift_id");
            entity.Property(e => e.WorkDate).HasColumnName("work_date");
            entity.Property(e => e.Remark).HasColumnName("remark");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrAttendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_attendance", tb => tb.HasComment("打卡/考勤明细（结果传递至薪资核算）")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.Property(e => e.WorkDate).HasColumnName("work_date");
            entity.Property(e => e.PunchIn).HasColumnName("punch_in");
            entity.Property(e => e.PunchOut).HasColumnName("punch_out");
            entity.Property(e => e.LateMin).HasColumnName("late_min");
            entity.Property(e => e.EarlyMin).HasColumnName("early_min");
            entity.Property(e => e.Absent).HasColumnName("absent");
            entity.Property(e => e.OvertimeMin).HasColumnName("overtime_min");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrLeave>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_leave", tb => tb.HasComment("请假单")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LeaveNo).HasColumnName("leave_no");
            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.LeaveType).HasColumnName("leave_type");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.Days).HasColumnName("days");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.ApprovalId).HasColumnName("approval_id");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrOvertime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_overtime", tb => tb.HasComment("加班单")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OtNo).HasColumnName("ot_no");
            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.OtDate).HasColumnName("ot_date");
            entity.Property(e => e.Hours).HasColumnName("hours");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.ApprovalId).HasColumnName("approval_id");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrSalaryItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_salary_item", tb => tb.HasComment("薪资项目（自定义）")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ItemNo).HasColumnName("item_no");
            entity.Property(e => e.ItemName).HasColumnName("item_name");
            entity.Property(e => e.ItemType).HasColumnName("item_type");
            entity.Property(e => e.CalcType).HasColumnName("calc_type");
            entity.Property(e => e.TaxFlag).HasColumnName("tax_flag");
            entity.Property(e => e.OrderNo).HasColumnName("order_no");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrEmployeeSalary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_employee_salary", tb => tb.HasComment("员工薪资档案(定薪)")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.BaseSalary).HasColumnName("base_salary");
            entity.Property(e => e.EffDate).HasColumnName("eff_date");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrSalary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_salary", tb => tb.HasComment("薪资核算主表")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SalaryNo).HasColumnName("salary_no");
            entity.Property(e => e.Period).HasColumnName("period");
            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.BaseSalary).HasColumnName("base_salary");
            entity.Property(e => e.AttendDays).HasColumnName("attend_days");
            entity.Property(e => e.OtPay).HasColumnName("ot_pay");
            entity.Property(e => e.LeaveDeduct).HasColumnName("leave_deduct");
            entity.Property(e => e.PerfBonus).HasColumnName("perf_bonus");
            entity.Property(e => e.GrossPay).HasColumnName("gross_pay");
            entity.Property(e => e.SocialPersonal).HasColumnName("social_personal");
            entity.Property(e => e.FundPersonal).HasColumnName("fund_personal");
            entity.Property(e => e.Tax).HasColumnName("tax");
            entity.Property(e => e.NetPay).HasColumnName("net_pay");
            entity.Property(e => e.VoucherId).HasColumnName("voucher_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrSalaryDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_salary_detail", tb => tb.HasComment("薪资明细")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SalaryId).HasColumnName("salary_id");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.ItemName).HasColumnName("item_name");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Direction).HasColumnName("direction");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrSocialRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_social_rule", tb => tb.HasComment("社保/公积金缴费规则")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.City).HasColumnName("city");
            entity.Property(e => e.BaseMin).HasColumnName("base_min");
            entity.Property(e => e.BaseMax).HasColumnName("base_max");
            entity.Property(e => e.PensionEmpRate).HasColumnName("pension_emp_rate");
            entity.Property(e => e.PensionComRate).HasColumnName("pension_com_rate");
            entity.Property(e => e.MedicalEmpRate).HasColumnName("medical_emp_rate");
            entity.Property(e => e.MedicalComRate).HasColumnName("medical_com_rate");
            entity.Property(e => e.UnemploymentEmpRate).HasColumnName("unemployment_emp_rate");
            entity.Property(e => e.UnemploymentComRate).HasColumnName("unemployment_com_rate");
            entity.Property(e => e.InjuryComRate).HasColumnName("injury_com_rate");
            entity.Property(e => e.BirthComRate).HasColumnName("birth_com_rate");
            entity.Property(e => e.FundEmpRate).HasColumnName("fund_emp_rate");
            entity.Property(e => e.FundComRate).HasColumnName("fund_com_rate");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrTaxBracket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_tax_bracket", tb => tb.HasComment("个税累进税率表(月度)")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.ThresholdLow).HasColumnName("threshold_low");
            entity.Property(e => e.ThresholdHigh).HasColumnName("threshold_high");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.QuickDeduction).HasColumnName("quick_deduction");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrWelfare>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_welfare", tb => tb.HasComment("员工福利")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.WelfareType).HasColumnName("welfare_type");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Month).HasColumnName("month");
            entity.Property(e => e.Remark).HasColumnName("remark");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrRecruitPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_recruit_plan", tb => tb.HasComment("人力需求/招聘计划")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PlanNo).HasColumnName("plan_no");
            entity.Property(e => e.DeptId).HasColumnName("dept_id");
            entity.Property(e => e.PosId).HasColumnName("pos_id");
            entity.Property(e => e.NeedCount).HasColumnName("need_count");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.ApprovalId).HasColumnName("approval_id");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrCandidate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_candidate", tb => tb.HasComment("候选人/人才库")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Education).HasColumnName("education");
            entity.Property(e => e.Source).HasColumnName("source");
            entity.Property(e => e.ResumeUrl).HasColumnName("resume_url");
            entity.Property(e => e.PosId).HasColumnName("pos_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.TalentPool).HasColumnName("talent_pool");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrInterview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_interview", tb => tb.HasComment("面试记录")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CandidateId).HasColumnName("candidate_id");
            entity.Property(e => e.Round).HasColumnName("round");
            entity.Property(e => e.InterviewDate).HasColumnName("interview_date");
            entity.Property(e => e.Interviewers).HasColumnName("interviewers");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.Result).HasColumnName("result");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrOffer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_offer", tb => tb.HasComment("录用offer")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CandidateId).HasColumnName("candidate_id");
            entity.Property(e => e.OfferNo).HasColumnName("offer_no");
            entity.Property(e => e.Salary).HasColumnName("salary");
            entity.Property(e => e.EffDate).HasColumnName("eff_date");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrPerfIndicator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_perf_indicator", tb => tb.HasComment("考核指标库")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IndNo).HasColumnName("ind_no");
            entity.Property(e => e.IndName).HasColumnName("ind_name");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.Weight).HasColumnName("weight");
            entity.Property(e => e.Target).HasColumnName("target");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrPerfPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_perf_plan", tb => tb.HasComment("考核计划")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PlanNo).HasColumnName("plan_no");
            entity.Property(e => e.PlanName).HasColumnName("plan_name");
            entity.Property(e => e.Period).HasColumnName("period");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrPerfResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_perf_result", tb => tb.HasComment("考核结果（应用至薪资/晋升）")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PlanId).HasColumnName("plan_id");
            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.Conclusion).HasColumnName("conclusion");
            entity.Property(e => e.ApplySalary).HasColumnName("apply_salary");
            entity.Property(e => e.ApplyPromote).HasColumnName("apply_promote");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrPerfDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_perf_detail", tb => tb.HasComment("考核明细（按指标）")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ResultId).HasColumnName("result_id");
            entity.Property(e => e.IndId).HasColumnName("ind_id");
            entity.Property(e => e.IndName).HasColumnName("ind_name");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.Weight).HasColumnName("weight");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrTrainNeed>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_train_need", tb => tb.HasComment("培训需求")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NeedNo).HasColumnName("need_no");
            entity.Property(e => e.DeptId).HasColumnName("dept_id");
            entity.Property(e => e.NeedDesc).HasColumnName("need_desc");
            entity.Property(e => e.Priority).HasColumnName("priority");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrTrainPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_train_plan", tb => tb.HasComment("培训计划")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PlanNo).HasColumnName("plan_no");
            entity.Property(e => e.PlanName).HasColumnName("plan_name");
            entity.Property(e => e.TrainType).HasColumnName("train_type");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.Trainer).HasColumnName("trainer");
            entity.Property(e => e.Budget).HasColumnName("budget");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrTrainRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_train_record", tb => tb.HasComment("培训记录")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PlanId).HasColumnName("plan_id");
            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.Hours).HasColumnName("hours");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.Certificate).HasColumnName("certificate");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<HrTrainExpense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("hr_train_expense", tb => tb.HasComment("培训费用")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PlanId).HasColumnName("plan_id");
            entity.Property(e => e.ExpenseType).HasColumnName("expense_type");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.OccurDate).HasColumnName("occur_date");
            entity.Property(e => e.Remark).HasColumnName("remark");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<MesWorkReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("mes_work_report", tb => tb.HasComment("报工单")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ReportNo).HasColumnName("report_no");
            entity.Property(e => e.ProductionOrderId).HasColumnName("production_order_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.GoodQty).HasColumnName("good_qty");
            entity.Property(e => e.BadQty).HasColumnName("bad_qty");
            entity.Property(e => e.WorkHours).HasColumnName("work_hours");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.ReportTime).HasColumnName("report_time");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<MesMaterialIssue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("mes_material_issue", tb => tb.HasComment("领料单")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IssueNo).HasColumnName("issue_no");
            entity.Property(e => e.WorkOrderId).HasColumnName("work_order_id");
            entity.Property(e => e.MaterialId).HasColumnName("material_id");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.IssueTime).HasColumnName("issue_time");
            entity.Property(e => e.Remark).HasColumnName("remark");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<MesProductionOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("mes_production_order", tb => tb.HasComment("生产任务(派工)")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProdNo).HasColumnName("prod_no");
            entity.Property(e => e.WorkOrderId).HasColumnName("work_order_id");
            entity.Property(e => e.WorkstationId).HasColumnName("workstation_id");
            entity.Property(e => e.PlanQty).HasColumnName("plan_qty");
            entity.Property(e => e.DoneQty).HasColumnName("done_qty");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.Remark).HasColumnName("remark");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<MesWorkstation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("mes_workstation", tb => tb.HasComment("工位/产线")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Line).HasColumnName("line");
            entity.Property(e => e.Remark).HasColumnName("remark");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<MesWorkOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("mes_work_order", tb => tb.HasComment("生产工单")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.PrOrderId).HasColumnName("pr_order_id");
            entity.Property(e => e.Source).HasColumnName("source");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OrderNo).HasColumnName("order_no");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.PlanQty).HasColumnName("plan_qty");
            entity.Property(e => e.DoneQty).HasColumnName("done_qty");
            entity.Property(e => e.Priority).HasColumnName("priority");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.PlanStart).HasColumnName("plan_start");
            entity.Property(e => e.PlanEnd).HasColumnName("plan_end");
            entity.Property(e => e.Remark).HasColumnName("remark");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<MesRouting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("mes_routing", tb => tb.HasComment("工艺路线")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RoutingCode).HasColumnName("routing_code");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Version).HasColumnName("version");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Remark).HasColumnName("remark");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<MesRoutingStep>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("mes_routing_step", tb => tb.HasComment("工艺工序")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RoutingId).HasColumnName("routing_id");
            entity.Property(e => e.StepNo).HasColumnName("step_no");
            entity.Property(e => e.StepName).HasColumnName("step_name");
            entity.Property(e => e.WorkstationId).HasColumnName("workstation_id");
            entity.Property(e => e.StdHours).HasColumnName("std_hours");
            entity.Property(e => e.Remark).HasColumnName("remark");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

        modelBuilder.Entity<MesQualityCheck>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("mes_quality_check", tb => tb.HasComment("质量检验")).UseCollation("utf8mb4_0900_ai_ci");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CheckNo).HasColumnName("check_no");
            entity.Property(e => e.ProductionOrderId).HasColumnName("production_order_id");
            entity.Property(e => e.WorkReportId).HasColumnName("work_report_id");
            entity.Property(e => e.CheckType).HasColumnName("check_type");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.PassQty).HasColumnName("pass_qty");
            entity.Property(e => e.FailQty).HasColumnName("fail_qty");
            entity.Property(e => e.Result).HasColumnName("result");
            entity.Property(e => e.CheckTime).HasColumnName("check_time");
            entity.Property(e => e.Remark).HasColumnName("remark");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
        });

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // 连接串统一通过依赖注入（Program.cs）从 appsettings.json 读取，源码不硬编码账号密码。
        // 数据库表结构由 SQL 脚本（SqlScript/erp_db*.sql）创建，禁止在后端代码里建表。
        if (!optionsBuilder.IsConfigured)
        {
            throw new InvalidOperationException(
                "未检测到 DbContext 配置。请通过依赖注入注册 ErpDbContext。");
        }
    }
}
