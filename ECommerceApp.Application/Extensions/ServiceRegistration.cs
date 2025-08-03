using ECommerceApp.Application.Interfaces;
using ECommerceApp.Application.Services;
using System.Net.Http;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.Persistence.Contexts;
using ECommerceApp.Application.Configurations;
using ECommerceApp.Domain.Repositories;
using ECommerceApp.Infrastructure.Configurations;
using Microsoft.Extensions.Options; 
using ECommerceApp.Infrastructure.Services;
using ECommerceApp.Infrastructure.Interfaces;
using ECommerceApp.Persistence;
using ECommerceApp.Persistence.Repositories;

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
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IDataContext, PostgreDataContext>((sp, options) =>
            {
                var config = sp.GetRequiredService<IOptions<AppSettings>>().Value;
                options.UseNpgsql(config.ConnectionStrings.Postgres);
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();
            
            services.AddScoped<IProductRepository, ProductRepository>();


            return services;
        }
        
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Redis
            var redisSettings = new RedisSettings();
            configuration.GetSection("Redis").Bind(redisSettings);
            services.AddSingleton(redisSettings);
            services.AddSingleton<ICacheService, RedisCacheService>();

            // Log
            services.AddSingleton<ILogService, LogstashService>();
            
            // Clients
            services.AddHttpClient<IBalanceApiClient, BalanceApiClient>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<BalanceApiSettings>>();
                client.BaseAddress = new Uri(options.Value.BalanceApiBaseUrl);
            });
            
            return services;
        }
    }
}