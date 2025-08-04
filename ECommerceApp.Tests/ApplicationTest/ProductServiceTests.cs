namespace ECommerceApp.Tests.ApplicationTest;

using ECommerceApp.Application.DTOs.Product;
using ECommerceApp.Application.Services;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Repositories;
using ECommerceApp.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;


[TestFixture]
public class ProductServiceTests
{
    private Mock<IProductRepository> _mockRepository = null!;
    private Mock<ICacheService> _mockCacheService = null!;
    private Mock<ILogService> _mockLogService = null!;
    private ProductService _productService = null!;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IProductRepository>();
        _mockCacheService = new Mock<ICacheService>();
        _mockLogService = new Mock<ILogService>();

        _productService = new ProductService(
            _mockRepository.Object,
            _mockCacheService.Object,
            _mockLogService.Object
        );
    }

    [Test]
    public async Task GetAllAsync_Should_Return_Products_From_Cache_If_Present()
    {
        // Arrange
        var cachedProducts = new List<ProductDto>
        {
            new ProductDto
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Price = 10,
                Currency = "USD",
                Stock = 100,
                Category = "Books"
            }
        };

        _mockCacheService
            .Setup(x => x.GetAsync<List<ProductDto>>("products:all"))
            .ReturnsAsync(cachedProducts);

        // Act
        var result = await _productService.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(cachedProducts);
        result.Error.Should().BeNull();

        _mockRepository.Verify(r => r.GetAllAsync(), Times.Never);
        _mockCacheService.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<List<ProductDto>>()), Times.Never);
        _mockLogService.Verify(l => l.Info(It.IsAny<string>()), Times.AtLeastOnce);
    }
}