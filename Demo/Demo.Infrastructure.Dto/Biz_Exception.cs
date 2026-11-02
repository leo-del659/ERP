namespace Demo.Infrastructure.Dto;

public class Biz_Exception : Exception
{
    /// <summary>业务错误码（可选）：透传至 Api_Response.bizCode</summary>
    public int BizCode { get; }

    public Biz_Exception(string message, int bizCode = 0) : base(message)
    {
        BizCode = bizCode;
    }
}
