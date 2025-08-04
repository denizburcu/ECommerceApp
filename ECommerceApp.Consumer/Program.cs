using ECommerceApp.Application.Extensions;
using MassTransit;
using ECommerceApp.Consumer.Consumers;
using ECommerceApp.Consumer.Extensions;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((ctx, cfg) =>
    {
        cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .ConfigureServices((ctx, services) =>
    {
        services.AddConsumerMessaging(ctx.Configuration);

        // Application servislerini de register et
        services.AddApplicationServices(ctx.Configuration);
        services.AddInfrastructureServices(ctx.Configuration);
    });

var host = builder.Build();
await host.RunAsync();    
