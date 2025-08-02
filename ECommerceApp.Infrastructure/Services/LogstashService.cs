using ECommerceApp.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace ECommerceApp.Infrastructure.Services;

public class LogstashService: ILogService
{
    private readonly Serilog.ILogger _logger;

    public LogstashService(IConfiguration configuration)
    {
        _logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.DurableHttpUsingFileSizeRolledBuffers(requestUri: "http://logstash:5044")
            .CreateLogger();
    }
    
    public void Info(string message)
    {
        _logger.Information(message);
    }

    public void Warn(string message)
    {
        _logger.Warning(message);
    }

    public void Error(string message, Exception? ex = null)
    {
        _logger.Error(ex, message);
    }
}