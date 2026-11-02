using Demo.Infrastructure.Attributes;
using Demo.Infrastructure.Dto;
using Demo.Infrastructure.Dto.Request;
using Demo.Infrastructure.EFCore;

namespace Demo.Service.Interface;

[Provider_Attribute]
public interface I_Auth_Service
{
    Task<object> LoginAsync(Login_Request req);
    SysUser? FindByName(string username);
    Task<List<string>> GetPathsAsync(long userId);
}

[Provider_Attribute]
public interface I_Log_Service
{
    Task Add_ActionAsync(SysOperLog log);
    Task Add_ExceptionAsync(SysExLog log);
    Task<object> PageOperAsync(Page_Request req);
    Task<object> PageExAsync(Page_Request req);
}

[Provider_Attribute]
public interface I_Md_Service
{
    Task<object> PageAsync<T>(Page_Request req, Func<IQueryable<T>, IQueryable<T>>? filter = null) where T : class;
    Task<object> SaveAsync<T>(T entity) where T : class;
    Task DeleteAsync<T>(long id) where T : class;
}

[Provider_Attribute]
public interface I_Inventory_Service
{
    Task<object> ConfirmInAsync(Inv_In_Request req);
    Task<object> ConfirmOutAsync(Inv_Out_Request req);
    Task ApplyQtyAsync(long materialId, long warehouseId, decimal qty, sbyte direction, string transType, string sourceNo, string? batchNo = null);
    Task<object> TransferAsync(InvTransfer bill);
    Task<object> StockPageAsync(Page_Request req, long? warehouseId);
    Task<object> TransPageAsync(Page_Request req);
    Task<object> AlertsAsync();

    // ---- 库存盘点 ----
    Task<object> CreateCheckAsync(Check_Create_Request req);
    Task<object> CheckPageAsync(Page_Request req);
    Task<object> CheckDetailAsync(long checkId);
    Task<object> FinishCheckAsync(Check_Finish_Request req);
    Task<object> AuditCheckAsync(long checkId);
    /// <summary>盘点审批驳回：退回盘点中并清除已录入实盘数</summary>
    Task<object> RejectCheckAsync(long checkId);

    // ---- 批次 / 库位 ----
    Task<object> BatchPageAsync(Page_Request req, long? warehouseId);
    Task<object> SaveBatchAsync(Batch_Save_Request req);
}

[Provider_Attribute]
public interface I_Sales_Service
{
    Task<object> CreateAsync(So_Create_Request req);
    Task<object> ConfirmAsync(long id);
    Task<object> DeliverAsync(Delivery_Create_Request req);
    Task<object> CompleteAsync(long id);
    Task<object> PageAsync(Page_Request req);

    // ---- 销售报价与价格策略 ----
    Task<object> CreateQuoteAsync(Quote_Create_Request req);
    Task<object> QuotePageAsync(Page_Request req);
    Task<object> QuoteDetailAsync(long id);
    Task<object> ConfirmQuoteAsync(long id);
    Task<object> QuoteToOrderAsync(Quote_To_Order_Request req);

    // ---- 订单变更 ----
    Task<object> ChangeAsync(Order_Change_Request req);
    Task<object> ChangeLogAsync(string billType, long billId);

    // ---- 发货与物流跟踪 ----
    Task<object> DeliveryPageAsync(Page_Request req);
    Task<object> AddTrackAsync(Track_Create_Request req);
    Task<object> TrackListAsync(long deliveryId);

    // ---- 客户信用管理 ----
    Task<object> CreditPageAsync(Page_Request req);
    Task<object> SaveCreditAsync(SoCustomerCredit credit);
    Task<object> FreezeCreditAsync(long customerId, bool freeze);
}

[Provider_Attribute]
public interface I_Purchase_Service
{
    Task<object> CreateAsync(Po_Create_Request req);
    Task<object> ApproveAsync(long id);
    Task<object> ReceiveAsync(Receipt_Create_Request req);
    Task<object> InspectAsync(long receiptId, bool pass);
    Task<object> PageAsync(Page_Request req);

    // ---- 采购订单变更 ----
    Task<object> ChangeAsync(Order_Change_Request req);

    // ---- 请购单 ----
    Task<object> CreateRequisitionAsync(Requisition_Create_Request req);
    Task<object> RequisitionPageAsync(Page_Request req);
    Task<object> RequisitionToPoAsync(Requisition_To_Po_Request req);

    // ---- 供应商报价与绩效 ----
    Task<object> SaveQuoteAsync(Supplier_Quote_Request req);
    Task<object> QuotePageAsync(Page_Request req, long? materialId);
    Task<Best_Price_Result?> BestPriceAsync(long materialId);
    Task<object> PerformanceAsync(long supplierId);
}

[Provider_Attribute]
public interface I_Production_Service
{
    Task<object> CreateOrderAsync(Pr_Order_Create_Request req);
    Task<object> ReleaseAsync(long id);
    Task<object> IssueAsync(Issue_Request req);
    Task<object> ReportAsync(Report_Request req);
    Task<object> CompleteAsync(long id, long warehouseId);
    Task<object> RunMrpAsync();
    Task<object> PageAsync(Page_Request req);

    // ---- 主生产计划 MPS ----
    Task<object> CreateMpsAsync(Mps_Create_Request req);
    Task<object> MpsPageAsync(Page_Request req);
    Task<object> ConfirmMpsAsync(long id);
    Task<object> MpsToOrderAsync(Mps_To_Order_Request req);

    // ---- MRP 运算结果 ----
    Task<object> MrpPageAsync(Page_Request req, string? mrpNo);
    Task<object> MrpToPoAsync(Mrp_To_Po_Request req);
}

[Provider_Attribute]
public interface I_Finance_Service
{
    Task<object> CreateVoucherAsync(Voucher_Create_Request req);
    Task<object> AuditVoucherAsync(long id);
    Task<object> PostVoucherAsync(long id);
    Task<object> CloseVoucherAsync(long id);
    Task<object> WriteoffReceiptAsync(Receipt_Writeoff_Request req);
    Task<object> WriteoffPaymentAsync(Payment_Writeoff_Request req);
    Task<object> AgeingAsync();
    Task<object> CalcCostAsync(Cost_Calc_Request req);
    Task<object> PageVoucherAsync(Page_Request req);
    Task<object> VoucherDetailAsync(long id);
    Task<object> PageReceivableAsync(Page_Request req);
    Task<object> PagePayableAsync(Page_Request req);
}

[Provider_Attribute]
public interface I_Pay_Service
{
    Task<object> NotifyAsync(Pay_Notify_Request req);
}

[Provider_Attribute]
public interface I_Dashboard_Service
{
    Task<object> OverviewAsync();
}
