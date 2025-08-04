namespace ECommerceApp.Infrastructure.Interfaces;
using StackExchange.Redis;

public interface ICacheService
{
    // <summary>
    /// GetDb
    /// </summary>
    IDatabase GetDb(int db = 0);
    
    // <summary>
    /// Stores the given value in the cache under the specified key.
    /// </summary>
    Task SetAsync<T>(string key, T value);

    /// <summary>
    /// Retrieves the value stored in the cache under the specified key.
    /// </summary>
    Task<T?> GetAsync<T>(string key);

    /// <summary>
    /// Removes the cached value associated with the specified key.
    /// </summary>
    Task RemoveAsync(string key);

    /// <summary>
    /// Acquires a distributed lock for the given key.
    /// </summary>
    Task<bool> AcquireLockAsync(string key, string value, TimeSpan expiration);

    /// <summary>
    /// Releases the distributed lock for the given key only if the value matches.
    /// </summary>
    Task<bool> ReleaseLockAsync(string key, string value);
}