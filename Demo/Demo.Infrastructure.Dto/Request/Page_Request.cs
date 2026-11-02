namespace Demo.Infrastructure.Dto.Request;

public class Page_Request
{
    public int page { get; set; } = 1;
    public int size { get; set; } = 20;
    public string? keyword { get; set; }

    public int SafePage => page < 1 ? 1 : page;

    public int SafeSize => size < 1 ? 10 : (size > 200 ? 200 : size);
}

public class Login_Request
{
    public string username { get; set; } = "";
    public string password { get; set; } = "";
}

public class Order_Line_Dto
{
    public long materialId { get; set; }
    public decimal quantity { get; set; }
    public decimal unitPrice { get; set; }
}

public class So_Create_Request
{
    public long customerId { get; set; }
    public DateOnly orderDate { get; set; }
    public DateOnly? deliveryDate { get; set; }
    public List<Order_Line_Dto> lines { get; set; } = new();
}

public class Po_Create_Request
{
    public long supplierId { get; set; }
    public DateOnly orderDate { get; set; }
    public List<Order_Line_Dto> lines { get; set; } = new();
}

public class Stock_Line_Dto
{
    public long materialId { get; set; }
    public decimal quantity { get; set; }

    /// <summary>批次号（可空；填写则同步登记批次库存）</summary>
    public string? batchNo { get; set; }

    /// <summary>库位ID（可空）</summary>
    public long? locationId { get; set; }
}

public class Inv_In_Request
{
    public string inType { get; set; } = "PO";
    public string? sourceNo { get; set; }
    public long warehouseId { get; set; }
    public DateOnly inDate { get; set; }
    public List<Stock_Line_Dto> lines { get; set; } = new();
}

public class Inv_Out_Request
{
    public string outType { get; set; } = "SO";
    public string? sourceNo { get; set; }
    public long warehouseId { get; set; }
    public DateOnly outDate { get; set; }
    public List<Stock_Line_Dto> lines { get; set; } = new();
}

public class Pay_Notify_Request
{
    public string notifyNo { get; set; } = "";
    public string channel { get; set; } = "mock";
    public long receivableId { get; set; }
    public decimal amount { get; set; }
    public string? bizNo { get; set; }
    public string? sign { get; set; }
}

public class Voucher_Entry_Dto
{
    public string accountCode { get; set; } = "";
    public string direction { get; set; } = "DR";
    public decimal amount { get; set; }
    public string? memo { get; set; }
}

public class Voucher_Create_Request
{
    public DateOnly voucherDate { get; set; }
    public string? sourceModule { get; set; }
    public long? sourceId { get; set; }
    public List<Voucher_Entry_Dto> entries { get; set; } = new();
}

/// <summary>HR 薪资核算请求</summary>
public class HrCalc_Request
{
    public int year { get; set; }
    public int month { get; set; }
    /// <summary>为空则核算所有在职员工</summary>
    public List<long>? employeeIds { get; set; }
}

public class Receipt_Create_Request
{
    public long orderId { get; set; }
    public DateOnly receiptDate { get; set; }
    public List<Stock_Line_Dto> lines { get; set; } = new();
}

public class Delivery_Create_Request
{
    public long orderId { get; set; }
    public long warehouseId { get; set; }
    public DateOnly deliveryDate { get; set; }
    public List<Stock_Line_Dto> lines { get; set; } = new();
}

public class Pr_Order_Create_Request
{
    public long materialId { get; set; }
    public decimal planQty { get; set; }
    public DateOnly? startDate { get; set; }
    public DateOnly? endDate { get; set; }
    public long? sourceSoId { get; set; }
    public long? sourceMpsId { get; set; }
}

public class Issue_Request
{
    public long prOrderId { get; set; }
    public long warehouseId { get; set; }
    public long materialId { get; set; }
    public decimal issueQty { get; set; }
}

public class Report_Request
{
    public long prOrderId { get; set; }
    public string? processName { get; set; }
    public decimal reportQty { get; set; }
    public decimal goodQty { get; set; }
    public decimal badQty { get; set; }
    public decimal? workHours { get; set; }
}

public class Cost_Calc_Request
{
    public long prOrderId { get; set; }
    public decimal laborCost { get; set; }
    public decimal overheadCost { get; set; }
}

public class Receipt_Writeoff_Request
{
    public long customerId { get; set; }
    public long receivableId { get; set; }
    public decimal amount { get; set; }
    public DateOnly receiptDate { get; set; }
}

public class Payment_Writeoff_Request
{
    public long supplierId { get; set; }
    public long payableId { get; set; }
    public decimal amount { get; set; }
    public DateOnly paymentDate { get; set; }
}

// ===================== 审批流 =====================

/// <summary>提交审批单请求</summary>
public class Approval_Submit_Request
{
    public string bizType { get; set; } = "OTHER";
    public long? bizId { get; set; }
    public string? billNo { get; set; }
    public string title { get; set; } = "";
    public decimal? amount { get; set; }
    public int totalNodes { get; set; } = 1;
    public string? remark { get; set; }
}

/// <summary>审批处理请求（通过/驳回）</summary>
public class Approval_Handle_Request
{
    /// <summary>动作：2通过/3驳回</summary>
    public sbyte action { get; set; } = 2;
    public string? opinion { get; set; }
}

// ===================== 审批规则 =====================

/// <summary>审批规则保存请求</summary>
public class Approval_Rule_Save_Request
{
    public long? id { get; set; }
    public string bizType { get; set; } = "";
    public decimal minAmount { get; set; }
    public decimal? maxAmount { get; set; }
    public int level { get; set; } = 1;
    public string nodeRoles { get; set; } = "admin";
    public sbyte? enabled { get; set; } = 1;
    public string? remark { get; set; }
}

// ===================== 库存盘点 =====================

/// <summary>创建盘点单请求</summary>
public class Check_Create_Request
{
    public long warehouseId { get; set; }
    public DateOnly checkDate { get; set; }
    /// <summary>盘点物料范围：为空则盘点该仓库全部物料</summary>
    public List<long>? materialIds { get; set; }
    public string? remark { get; set; }
}

/// <summary>录入实盘数请求</summary>
public class Check_Finish_Request
{
    public long checkId { get; set; }
    public List<Check_Line_Dto> lines { get; set; } = new();
}

public class Check_Line_Dto
{
    public long materialId { get; set; }
    public decimal actualQty { get; set; }
}

// ===================== 批次 / 库位 =====================

/// <summary>批次登记请求</summary>
public class Batch_Save_Request
{
    public long materialId { get; set; }
    public long warehouseId { get; set; }
    public string batchNo { get; set; } = "";
    public decimal quantity { get; set; }
    public DateOnly? productionDate { get; set; }
    public DateOnly? expiryDate { get; set; }
}

// ===================== 生产计划 MPS =====================

/// <summary>新建主生产计划请求</summary>
public class Mps_Create_Request
{
    public long materialId { get; set; }
    public decimal planQty { get; set; }
    public DateOnly? planStartDate { get; set; }
    public DateOnly? planEndDate { get; set; }
    public string? sourceType { get; set; }
    public long? sourceId { get; set; }
    public string? remark { get; set; }
}

/// <summary>MPS 转生产订单请求</summary>
public class Mps_To_Order_Request
{
    public long mpsId { get; set; }
    public long warehouseId { get; set; }
}

/// <summary>MRP 建议转采购订单请求</summary>
public class Mrp_To_Po_Request
{
    public long mrpId { get; set; }
    public long supplierId { get; set; }
    public DateOnly? orderDate { get; set; }
}

// ===================== 销售报价 / 物流 =====================

public class Quote_Line_Dto
{
    public long materialId { get; set; }
    public decimal quantity { get; set; }
    public decimal unitPrice { get; set; }
    public decimal? discountRate { get; set; }
}

/// <summary>销售报价单请求</summary>
public class Quote_Create_Request
{
    public long customerId { get; set; }
    public DateOnly quoteDate { get; set; }
    public DateOnly? validDate { get; set; }
    /// <summary>价格策略：STANDARD/PREFER/VIP/CUSTOM</summary>
    public string priceStrategy { get; set; } = "STANDARD";
    public decimal? discountRate { get; set; } = 1m;
    public List<Quote_Line_Dto> lines { get; set; } = new();
    public string? remark { get; set; }
}

/// <summary>报价转销售订单请求</summary>
public class Quote_To_Order_Request
{
    public long quoteId { get; set; }
    public DateOnly? deliveryDate { get; set; }
}

/// <summary>物流跟踪请求</summary>
public class Track_Create_Request
{
    public long deliveryId { get; set; }
    public string? carrier { get; set; }
    public string? trackNo { get; set; }
    public string? trackStatus { get; set; }
    public string? location { get; set; }
    public string? remark { get; set; }
}

// ===================== 采购请购 / 供应商报价 =====================

public class Requisition_Line_Dto
{
    public long materialId { get; set; }
    public decimal reqQty { get; set; }
    public string? remark { get; set; }
}

/// <summary>请购单请求</summary>
public class Requisition_Create_Request
{
    public DateOnly reqDate { get; set; }
    public string? deptName { get; set; }
    public List<Requisition_Line_Dto> lines { get; set; } = new();
    public string? remark { get; set; }
}

/// <summary>请购转采购订单请求</summary>
public class Requisition_To_Po_Request
{
    public long reqId { get; set; }
    public long supplierId { get; set; }
    public DateOnly? orderDate { get; set; }
}

/// <summary>供应商报价请求</summary>
public class Supplier_Quote_Request
{
    public long supplierId { get; set; }
    public long materialId { get; set; }
    public decimal quotePrice { get; set; }
    public DateOnly? validFrom { get; set; }
    public DateOnly? validTo { get; set; }
}

// ===================== 单据变更 =====================

public class Order_Change_Line_Dto
{
    public long materialId { get; set; }
    public decimal? quantity { get; set; }
    public decimal? unitPrice { get; set; }
}

/// <summary>订单变更请求（销售 SO / 采购 PO 通用）</summary>
public class Order_Change_Request
{
    public long orderId { get; set; }
    /// <summary>变更后的完整明细（缺失的行视为删除，新增行视为新增）</summary>
    public List<Order_Change_Line_Dto> lines { get; set; } = new();
    public DateOnly? deliveryDate { get; set; }
    public string? reason { get; set; }
}
