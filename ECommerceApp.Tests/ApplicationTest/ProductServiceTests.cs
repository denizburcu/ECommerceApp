namespace ECommerceApp.Tests.Application.Services;

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
    private Mock<IProductRepository> _productRepositoryMock = null!;
    private Mock<ICacheService> _cacheServiceMock = null!;
    private Mock<ILogService> _loggerMock = null!;
    private ProductService _productService = null!;

    [SetUp]
    public void Setup()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _cacheServiceMock = new Mock<ICacheService>();
        _loggerMock = new Mock<ILogService>();

        _productService = new ProductService(
            _productRepositoryMock.Object,
            _cacheServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Test]
    public async Task GetAllAsync_Should_ReturnFromCache_When_CacheExists()
    {
        var cachedProducts = new List<ProductDto> { new ProductDto { Id = Guid.NewGuid(), Name = "Test" } };
        _cacheServiceMock.Setup(x => x.GetAsync<List<ProductDto>>("products:all"))
            .ReturnsAsync(cachedProducts);

        var result = await _productService.GetAllAsync();

        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(cachedProducts);
    }

    [Test]
    public async Task GetAllAsync_Should_ReturnFromDb_When_CacheMisses()
    {
        _cacheServiceMock.Setup(x => x.GetAsync<List<ProductDto>>("products:all"))
            .ReturnsAsync((List<ProductDto>?)null);

        var dbProducts = new List<ProductEntity>
        {
            new ProductEntity { Id = Guid.NewGuid(), Name = "DB Product" }
        };
        _productRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(dbProducts);

        var result = await _productService.GetAllAsync();

        result.Succeeded.Should().BeTrue();
        result.Data.Should().HaveCount(1);
        result.Data.Should().Contain(x => x.Name == "DB Product");
    }

    [Test]
    public async Task GetByIdAsync_Should_ReturnMappedProduct()
    {
        var id = Guid.NewGuid();
        _productRepositoryMock.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(new ProductEntity { Id = id, Name = "ById" });

        var result = await _productService.GetByIdAsync(id);

        result.Should().NotBeNull();
        result.Id.Should().Be(id);
    }

    [Test]
    public async Task GetByExternalIdAsync_Should_ReturnMappedProduct()
    {
        var externalId = "external-123";
        _productRepositoryMock.Setup(x => x.GetByExternalIdAsync(externalId))
            .ReturnsAsync(new ProductEntity { Id = Guid.NewGuid(), Name = "External Product" });

        var result = await _productService.GetByExternalIdAsync(externalId);

        result.Should().NotBeNull();
        result.Name.Should().Be("External Product");
    }

    [Test]
    public async Task CreateAsync_Should_AddProduct_And_ReturnDto()
    {
        var request = new CreateProductRequest { Name = "New Product" };
        _productRepositoryMock.Setup(x => x.AddAsync(It.IsAny<ProductEntity>()))
            .Returns(Task.CompletedTask);
        _productRepositoryMock.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(true);

        var result = await _productService.CreateAsync(request);

        result.Should().NotBeNull();
        result.Name.Should().Be("New Product");
    }

    [Test]
    public async Task UpdateAsync_Should_ReturnFalse_When_ProductNotFound()
    {
        _productRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((ProductEntity?)null);

        var result = await _productService.UpdateAsync(Guid.NewGuid(), new UpdateProductRequest());

        result.Should().BeFalse();
    }

    [Test]
    public async Task UpdateAsync_Should_UpdateAndReturnTrue_When_ProductExists()
    {
        var product = new ProductEntity { Id = Guid.NewGuid(), Name = "Old" };
        _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id))
            .ReturnsAsync(product);
        _productRepositoryMock.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(true);

        var result = await _productService.UpdateAsync(product.Id, new UpdateProductRequest { Name = "Updated" });

        result.Should().BeTrue();
    }

    [Test]
    public async Task DeleteAsync_Should_ReturnFalse_When_ProductNotFound()
    {
        _productRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((ProductEntity?)null);

        var result = await _productService.DeleteAsync(Guid.NewGuid());

        result.Should().BeFalse();
    }

    [Test]
    public async Task DeleteAsync_Should_DeleteAndReturnTrue_When_ProductExists()
    {
        var product = new ProductEntity { Id = Guid.NewGuid() };
        _productRepositoryMock.Setup(x => x.GetByIdAsync(product.Id))
            .ReturnsAsync(product);
        _productRepositoryMock.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(true);

        var result = await _productService.DeleteAsync(product.Id);

        result.Should().BeTrue();
    }
}
