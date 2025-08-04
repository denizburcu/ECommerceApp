namespace ECommerceApp.Api.Middlewares;

using System.Text;
using Microsoft.AspNetCore.Http;
using ECommerceApp.Infrastructure.Interfaces;

/// <summary>
/// Logs HTTP request and response data for debugging and monitoring.
/// </summary>
public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogService _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogService logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Request.EnableBuffering();

        var requestBodyStream = new MemoryStream();
        await context.Request.Body.CopyToAsync(requestBodyStream);
        requestBodyStream.Seek(0, SeekOrigin.Begin);
        var requestBody = await new StreamReader(requestBodyStream).ReadToEndAsync();
        requestBodyStream.Seek(0, SeekOrigin.Begin);
        context.Request.Body.Position = 0;

        _logger.Info($"[Request] {context.Request.Method} {context.Request.Path}\nBody: {requestBody}");

        var originalResponseBodyStream = context.Response.Body;
        using var tempResponseBody = new MemoryStream();
        context.Response.Body = tempResponseBody;

        await _next(context); // middleware pipeline çağrısı

        tempResponseBody.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(tempResponseBody).ReadToEndAsync();
        tempResponseBody.Seek(0, SeekOrigin.Begin);

        _logger.Info($"[Response] {context.Response.StatusCode}\nBody: {responseBody}");

        await tempResponseBody.CopyToAsync(originalResponseBodyStream);
        context.Response.Body = originalResponseBodyStream;
    }
}