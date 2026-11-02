namespace Demo.Api.Admin.Middles;

public class RequestBuffer_MiddleWare
{
    private readonly RequestDelegate _next;
    public RequestBuffer_MiddleWare(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();
        await _next(context);
    }
}
