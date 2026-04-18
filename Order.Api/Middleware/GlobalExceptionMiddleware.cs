using System.Net;
using System.Text.Json;

namespace Order.Api.Middleware;

internal sealed class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var correlationId = context.Items[CorrelationIdMiddleware.ItemsKey] as string;

            logger.LogError(ex,
                $"Unhandled exception occurred. TraceId: {context.TraceIdentifier} | CorrelationId: {correlationId}");

            await WriteErrorResponseAsync(context, ex);
        }
    }

    private static Task WriteErrorResponseAsync(HttpContext context, Exception ex)
    {
        var (statusCode, message) = ex switch
        {
            ArgumentException => (HttpStatusCode.BadRequest, "Invalid request."),
            InvalidOperationException => (HttpStatusCode.UnprocessableEntity, "Operation could not be completed."),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized."),
            KeyNotFoundException => (HttpStatusCode.NotFound, "Resource not found."),
            NotImplementedException => (HttpStatusCode.NotImplemented, "Not implemented."),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";

        var body = JsonSerializer.Serialize(
            new { error = message, traceId = context.TraceIdentifier },
            _jsonOptions);

        return context.Response.WriteAsync(body);
    }
}
