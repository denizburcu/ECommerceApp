namespace ECommerceApp.Tests.Application.Services;

using ECommerceApp.Application.Interfaces;
using ECommerceApp.Domain.DTOs.External;
using ECommerceApp.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;


[TestFixture]
public class OrderServiceTests
{
    private Mock<IBalanceApiClient> _balanceApiClientMock = null!;
    private Mock<ILogService> _loggerMock = null!;
    private Mock<IProductService> _productServiceMock = null!;
    private Mock<ICacheService> _cacheServiceMock = null!;
    private IOrderService _orderService = null!;

    [SetUp]
    public void Setup()
    {
        _balanceApiClientMock = new Mock<IBalanceApiClient>();
        _loggerMock = new Mock<ILogService>();
        _productServiceMock = new Mock<IProductService>();
        _cacheServiceMock = new Mock<ICacheService>();

        _orderService = new ECommerceApp.Application.Services.OrderService(
            _balanceApiClientMock.Object,
            _loggerMock.Object,
            null!,
            _productServiceMock.Object,
            _cacheServiceMock.Object);
    }

    [Test]
    public async Task CompleteOrderAsync_Should_ReturnSuccess_When_LockAcquired_And_ExternalApiSuccess()
    {
        // Arrange
        string orderId = "test-order-1";
        _cacheServiceMock.Setup(x => x.AcquireLockAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()))
            .ReturnsAsync(true);

        var apiResponse = new CompleteOrderResponseDto
        {
            Success = true,
            Data = new CompleteOrderDataDto
            {
                Order = new PreOrderDto { OrderId = orderId },
                UpdatedBalance = new UpdatedBalanceDto { UserId = "user-1" }
            }
        };

        _balanceApiClientMock
            .Setup(x => x.CompleteOrderAsync(It.Is<CompleteOrderRequestDto>(r => r.OrderId == orderId)))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _orderService.CompleteOrderAsync(orderId);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.PreOrder.OrderId.Should().Be(orderId);
        result.Data.UpdatedBalance.UserId.Should().Be("user-1");
    }

    [Test]
    public async Task CompleteOrderAsync_Should_ReturnFailure_When_LockNotAcquired()
    {
        // Arrange
        string orderId = "locked-order";
        _cacheServiceMock.Setup(x => x.AcquireLockAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()))
            .ReturnsAsync(false);

        // Act
        var result = await _orderService.CompleteOrderAsync(orderId);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Error!.Code.Should().Be("ORDER_LOCKED");
    }

    [Test]
    public async Task CompleteOrderAsync_Should_ReturnFailure_When_ExternalServiceThrowsException()
    {
        // Arrange
        string orderId = "exception-order";
        _cacheServiceMock.Setup(x => x.AcquireLockAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()))
            .ReturnsAsync(true);

        _balanceApiClientMock
            .Setup(x => x.CompleteOrderAsync(It.IsAny<CompleteOrderRequestDto>()))
            .ThrowsAsync(new Exception("Service unavailable"));

        // Act
        var result = await _orderService.CompleteOrderAsync(orderId);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Error!.Code.Should().Be("COMPLETE_ORDER_FAILED");
    }
}
