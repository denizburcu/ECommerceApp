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
    
    /// <summary>
    /// Acquires a distributed lock for the given key.
    /// </summary>
    public async Task<bool> AcquireLockAsync(string key, string value, TimeSpan expiration)
    {
        return await GetDb().StringSetAsync(
            key,
            value,
            expiration,
            when: When.NotExists); // SETNX
    }

    /// <summary>
    /// Releases the distributed lock for the given key only if the value matches.
    /// </summary>
    public async Task<bool> ReleaseLockAsync(string key, string value)
    {
        var script = @"
            if redis.call('get', KEYS[1]) == ARGV[1]
            then
                return redis.call('del', KEYS[1])
            else
                return 0
            end";

        var result = await GetDb().ScriptEvaluateAsync(
            script,
            keys: new RedisKey[] { key },
            values: new RedisValue[] { value });

        return (int)result == 1;
    }
}