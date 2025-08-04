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
}