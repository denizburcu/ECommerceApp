using ECommerceApp.Infrastructure.Configurations;
using ECommerceApp.Infrastructure.Interfaces;
using ECommerceApp.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using NUnit.Framework;

namespace ECommerceApp.Tests.IntegrationTests.Infrastructure;

[TestFixture]
public class RedisCacheServiceTests
{
    private ICacheService _cacheService = null!;
    private IDatabase _redisDb = null!;
    private string _testKey = "TestKey";
    private string _testValue = "TestValue";

    [SetUp]
    public void Setup()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var redisSettings = configuration.GetSection("RedisSettings").Get<RedisSettings>()!;

        var options = Options.Create(redisSettings);
        _cacheService = new RedisCacheService(options);
        _redisDb = _cacheService.GetDb();
    }

    [TearDown]
    public void Cleanup()
    {
        _redisDb.KeyDelete(_testKey);
    }

    [Test]
    public void Should_Set_And_Get_Value_From_Redis()
    {
        // Arrange
        _redisDb.StringSet(_testKey, _testValue);

        // Act
        var result = _redisDb.StringGet(_testKey);

        // Assert
        Assert.IsTrue(result.HasValue);
        Assert.AreEqual(_testValue, result.ToString());
    }
} 