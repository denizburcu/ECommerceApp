using ECommerceApp.Application.Interfaces;
using ECommerceApp.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.Persistence.Contexts;
using ECommerceApp.Application.Configurations;
using ECommerceApp.Infrastructure.Configurations;
using Microsoft.Extensions.Options; 
using ECommerceApp.Infrastructure.Services;
using ECommerceApp.Infrastructure.Interfaces;

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
        /// <param name="services">The service collection to add services to.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IDataContext, PostgreDataContext>((sp, options) =>
            {
                var config = sp.GetRequiredService<IOptions<AppSettings>>().Value;
                options.UseNpgsql(config.ConnectionStrings.Postgres);
            });

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
            
            return services;
        }
    }
}