namespace Demo.Infrastructure.Dto.Response;

public enum Api_Code
{
    fail = 0,
    success = 1,
    error = -1
}

public class Api_Response
{
    public Api_Code code { get; set; } = Api_Code.success;
    public string message { get; set; } = "ok";
    public object? data { get; set; }

    /// <summary>业务错误码（可选）：Biz_Exception 抛出时透传，便于前端按码处理</summary>
    public int? bizCode { get; set; }

    public static Api_Response Ok(object? data = null, string message = "ok")
        => new() { code = Api_Code.success, message = message, data = data };

    public static Api_Response Fail(string message, object? data = null, int? bizCode = null)
        => new() { code = Api_Code.fail, message = message, data = data, bizCode = bizCode };

    public static Api_Response Error(string message = "操作异常")
        => new() { code = Api_Code.error, message = message };
}

public class Page_Result
{
    public int total { get; set; }
    public object? rows { get; set; }
}
