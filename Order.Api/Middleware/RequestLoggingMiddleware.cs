namespace Order.Api.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Items[CorrelationIdMiddleware.ItemsKey] as string;
        var protocol = context.Request.Protocol;
        var method = context.Request.Method;
        var path = context.Request.Path;
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            await next(context);
            stopwatch.Stop();

            logger.LogInformation(
                "{Protocol} {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds}ms | CorrelationId: {CorrelationId}",
                protocol, method, path, context.Response.StatusCode, stopwatch.ElapsedMilliseconds, correlationId);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            logger.LogError(ex,
                "{Protocol} {Method} {Path} threw {ExceptionType} after {ElapsedMilliseconds}ms | CorrelationId: {CorrelationId}",
                protocol, method, path, ex.GetType().Name, stopwatch.ElapsedMilliseconds, correlationId);

            throw;
        }
    }
}
