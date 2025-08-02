namespace ECommerceApp.Application.Configurations;

/// <summary>
/// Application-wide configuration settings.
/// </summary>
public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; } = new();
    public RabbitMqSettings RabbitMq { get; set; } = new();
    public ElasticsearchSettings Elasticsearch { get; set; } = new();
}

public class ConnectionStrings
{
    public string Postgres { get; set; } = string.Empty;
}

public class RabbitMqSettings
{
    public string HostName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int Port { get; set; }
}

public class ElasticsearchSettings
{
    public string Uri { get; set; } = string.Empty;
}
