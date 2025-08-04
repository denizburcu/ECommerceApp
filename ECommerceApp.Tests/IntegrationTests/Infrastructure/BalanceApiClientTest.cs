namespace ECommerceApp.Tests.IntegrationTests.Infrastructure;

using System.Text.Json;
using ECommerceApp.Domain.DTOs.External;
using ECommerceApp.Infrastructure.Configurations;
using ECommerceApp.Infrastructure.Interfaces;
using ECommerceApp.Infrastructure.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

[TestFixture]
public class BalanceApiClientTests
{
    private IBalanceApiClient _balanceApiClient = null!;
    private Mock<ILogService> _mockLogService = null!;

    [SetUp]
    public void SetUp()
    {
        _mockLogService = new Mock<ILogService>();
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var balanceApiSettings = configuration.GetSection("ExternalServices").Get<BalanceApiSettings>()!;

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(balanceApiSettings.BalanceApiBaseUrl)
        };

        _balanceApiClient = new BalanceApiClient(httpClient, _mockLogService.Object);
    }

    [Test]
    public async Task GetProductsAsync_Should_Return_Valid_Product_List()
    {
        var products = await _balanceApiClient.GetProductsAsync();

        products.Should().NotBeNull();
        products.Should().AllBeOfType<ProductResponseDto>();
    }
}