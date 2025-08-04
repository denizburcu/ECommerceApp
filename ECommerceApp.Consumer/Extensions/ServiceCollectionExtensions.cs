using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ECommerceApp.Consumer.Consumers;

namespace ECommerceApp.Consumer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsumerMessaging(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<CreateOrderConsumer>();

            // Broker türünü config'ten oku (RabbitMQ, Kafka, Azure, vs.)
            var transport = configuration["Messaging:Transport"]?.ToLowerInvariant();

            x.UsingRabbitMq((context, cfg) =>
            {
                var rmq = configuration.GetSection("RabbitMq");

                cfg.Host(rmq["Host"], rmq["VirtualHost"], h =>
                {
                    h.Username(rmq["Username"]);
                    h.Password(rmq["Password"]);
                });

                // Kuyruk ismini config'ten al
                var queueName = configuration["Messaging:Consumers:CreateOrder:QueueName"];
                
                cfg.ReceiveEndpoint(queueName, e =>
                {
                    var retryCount = int.TryParse(configuration["Messaging:Consumers:CreateOrder:Retry:Count"], out var count) ? count : 3;
                    var intervalSeconds = int.TryParse(configuration["Messaging:Consumers:CreateOrder:Retry:IntervalSeconds"], out var seconds) ? seconds : 5;

                    e.UseMessageRetry(retry =>
                    {
                        retry.Interval(retryCount, TimeSpan.FromSeconds(intervalSeconds));
                    });
                    
                    e.SetQueueArgument("x-queue-type", "quorum"); 
                    e.ConfigureConsumer<CreateOrderConsumer>(context);
                });
            });
        });

        services.AddMassTransitHostedService();
        return services;
    }
}