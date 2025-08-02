using ECommerceApp.Infrastructure.Configurations;
using StackExchange.Redis;
using ECommerceApp.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;

namespace ECommerceApp.Infrastructure.Services
{
    public class RedisCacheService: ICacheService
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        
        public RedisCacheService(IOptions<RedisSettings> options)
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(options.Value.ConnectionString);
        }

        public IDatabase GetDb(int db = 0) => _connectionMultiplexer.GetDatabase(db);

    }
}
