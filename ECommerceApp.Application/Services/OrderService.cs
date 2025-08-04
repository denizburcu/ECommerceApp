using System;
using System.Threading.Tasks;
using ECommerceApp.Application.Common;
using ECommerceApp.Application.DTOs.Order;
using ECommerceApp.Application.Interfaces;
using ECommerceApp.Domain.DTOs.External;
using ECommerceApp.Infrastructure.Interfaces;
using ECommerceApp.Shared.Commands;
using Mapster;
using MassTransit;

namespace ECommerceApp.Application.Services;

/// <summary>
/// Provides business logic for managing orders.
/// </summary>
public class OrderService : IOrderService
{
    private readonly IBalanceApiClient _balanceApiClient;
    private readonly ILogService _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IProductService _productService;
    private readonly ICacheService _cacheService;

    public OrderService(IBalanceApiClient balanceApiClient, ILogService logger, IPublishEndpoint publishEndpoint,
        IProductService productService,  ICacheService cacheService)
    {
        _balanceApiClient = balanceApiClient;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _productService = productService;
        _cacheService = cacheService;
    }
    /// <summary>
    /// EnqueueOrderAsync
    /// </summary>
    public async Task<ServiceResult<bool>> EnqueueOrderAsync(CreateOrderRequest request)
    {
        var command = new CreateOrderCommand
        {
            OrderId = request.OrderId,
            ProductId = request.ProductId,
            Amount = request.Amount,
            CreatedAt = DateTime.UtcNow
        };

        await _publishEndpoint.Publish(command);

        return ServiceResult<bool>.Success();
    }

    /// <summary>
    /// Creates a pre-order by reserving the amount using Balance API.
    /// </summary>
    public async Task<ServiceResult<CreateOrderResponse>> CreateOrderAsync(CreateOrderRequest request)
    {
        var lockKey = $"order-lock:{request.OrderId}";
        var lockValue = Guid.NewGuid().ToString();
        var lockExpiration = TimeSpan.FromSeconds(10);

        var lockAcquired = await _cacheService.AcquireLockAsync(lockKey, lockValue, lockExpiration);

        if (!lockAcquired)
        {
            return ServiceResult<CreateOrderResponse>.Failure("DUPLICATE_ORDER", "Order is already being processed.");
        }
        
        try
        {
            var externalRequest = new PreOrderRequestDto()
            {
                OrderId = request.OrderId,
                Amount = request.Amount
            };
            
            var externalResponse = await _balanceApiClient.PreOrderAsync(externalRequest);

            var response = new CreateOrderResponse
            {
                PreOrder = externalResponse.Data.PreOrder.Adapt<PreOrderDto>(),
                UpdatedBalance = externalResponse.Data.UpdatedBalance.Adapt<UpdatedBalanceDto>()
            };

            return ServiceResult<CreateOrderResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.Error("Createorder client exception.", ex);
            
            return ServiceResult<CreateOrderResponse>.Failure(
                ServiceErrorCodes.ExternalServiceError,
                "Failed to create pre-order through Balance API.");
        }
    }

    /// <summary>
    /// completes a order using Balance API.
    /// </summary>
    public async Task<ServiceResult<CompleteOrderResponse>> CompleteOrderAsync(string orderId)
    {
        var lockKey = $"complete-order-lock:{orderId}";
        var lockValue = Guid.NewGuid().ToString();
        var lockExpiration = TimeSpan.FromSeconds(10); // Config'e alınabilir

        var lockAcquired = await _cacheService.AcquireLockAsync(lockKey, lockValue, lockExpiration);

        if (!lockAcquired)
        {
            return ServiceResult<CompleteOrderResponse>.Failure("ORDER_LOCKED", "Order is currently being completed by another process.");
        }
        
        try
        {
            var result = await _balanceApiClient.CompleteOrderAsync(new CompleteOrderRequestDto
            {
                OrderId = orderId
            });

            var response = new CompleteOrderResponse
            {
                PreOrder = result.Data.Order.Adapt<PreOrderDto>(),
                UpdatedBalance = result.Data.UpdatedBalance.Adapt<UpdatedBalanceDto>()
            };

            _logger.Info("Order completed successfully for OrderId: {OrderId}");

            return ServiceResult<CompleteOrderResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to complete order for OrderId: {OrderId}", ex);
            return ServiceResult<CompleteOrderResponse>.Failure("COMPLETE_ORDER_FAILED",
                "Failed to complete the order.");
        }
    }
}