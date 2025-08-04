namespace ECommerceApp.Infrastructure.Services;

using System.Text.Json;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using ECommerceApp.Infrastructure.Configurations;
using ECommerceApp.Infrastructure.Interfaces;

/// <summary>
/// Redis-based implementation of ICacheService.
/// </summary>
public class RedisCacheService : ICacheService
{
    private readonly ConnectionMultiplexer _connectionMultiplexer;

    public RedisCacheService(IOptions<RedisSettings> options)
    {
        _connectionMultiplexer = ConnectionMultiplexer.Connect(options.Value.ConnectionString);
    }

    /// <summary>
    /// Gets the Redis database instance.
    /// </summary>
    public IDatabase GetDb(int db = 0) => _connectionMultiplexer.GetDatabase(db);

    /// <summary>
    /// Stores the given value in Redis under the specified key.
    /// </summary>
    public async Task SetAsync<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value);
        await GetDb().StringSetAsync(key, json);
    }

    /// <summary>
    /// Retrieves a value from Redis based on the specified key.
    /// </summary>
    public async Task<T?> GetAsync<T>(string key)
    {
        var json = await GetDb().StringGetAsync(key);
        return json.HasValue
            ? JsonSerializer.Deserialize<T>(json!)
            : default;
    }

    /// <summary>
    /// Removes a cached value from Redis using the specified key.
    /// </summary>
    public async Task RemoveAsync(string key)
    {
        await GetDb().KeyDeleteAsync(key);
    }
}