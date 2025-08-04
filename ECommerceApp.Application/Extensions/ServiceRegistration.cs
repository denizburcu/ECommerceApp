using System.Net.Http;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ECommerceApp.Application.Services;
using ECommerceApp.Application.Interfaces;
using ECommerceApp.Application.Configurations;
using ECommerceApp.Domain.Repositories;
using ECommerceApp.Persistence;
using ECommerceApp.Persistence.Contexts;
using ECommerceApp.Persistence.Repositories;
using ECommerceApp.Infrastructure.Services;
using ECommerceApp.Infrastructure.Interfaces;
using ECommerceApp.Infrastructure.Configurations;

namespace ECommerceApp.Application.Extensions
{
    /// <summary>
    /// Provides extension methods to register application layer services.
    /// </summary>
    public static class ServiceRegistration
    {
        /// <summary>
        /// Registers application services in the dependency injection container.
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<IDataContext, PostgreDataContext>((sp, options) =>
            {
                var config = sp.GetRequiredService<IOptions<AppSettings>>().Value;
                options.UseNpgsql(config.ConnectionStrings.Postgres);
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<IProductRepository, ProductRepository>();
            
            return services;
        }

        /// <summary>
        /// Registers infrastructure services in the dependency injection container.
        /// </summary>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            // Redis
            var redisSettings = new RedisSettings();
            configuration.GetSection("Redis").Bind(redisSettings);
            services.AddSingleton(redisSettings);
            services.AddSingleton<ICacheService, RedisCacheService>();

            // Log
            services.AddSingleton<ILogService, LogstashService>();

            // Clients
            services.Configure<BalanceApiSettings>(configuration.GetSection("ExternalServices"));
            services.AddHttpClient<IBalanceApiClient, BalanceApiClient>((sp, client) =>
            {
                var config = sp.GetRequiredService<IOptions<BalanceApiSettings>>().Value;

                if (string.IsNullOrWhiteSpace(config.BalanceApiBaseUrl))
                    throw new InvalidOperationException("BalanceApiBaseUrl is not configured properly.");

                client.BaseAddress = new Uri(config.BalanceApiBaseUrl);
            });

            return services;
        }
    }
}