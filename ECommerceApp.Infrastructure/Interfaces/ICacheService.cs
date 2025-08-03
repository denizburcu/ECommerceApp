using StackExchange.Redis;

namespace ECommerceApp.Infrastructure.Interfaces;

public interface ICacheService
{
    IDatabase GetDb(int db = 0);

}