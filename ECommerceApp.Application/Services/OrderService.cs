using System;
using System.Threading.Tasks;
using ECommerceApp.Application.Common;
using ECommerceApp.Application.DTOs.Order;
using ECommerceApp.Application.Interfaces;
using ECommerceApp.Domain.DTOs.External;
using ECommerceApp.Infrastructure.Interfaces;
using Mapster;

namespace ECommerceApp.Application.Services;

/// <summary>
/// Provides business logic for managing orders.
/// </summary>
public class OrderService : IOrderService
{
    private readonly IBalanceApiClient _balanceApiClient;
    private readonly ILogService _logger;

    public OrderService(IBalanceApiClient balanceApiClient, ILogService logger)
    {
        _balanceApiClient = balanceApiClient;
        _logger = logger;
    }
    /// <summary>
    /// Creates a pre-order by reserving the amount using Balance API.
    /// </summary>
    public async Task<ServiceResult<CreateOrderResponse>> CreateOrderAsync(CreateOrderRequest request)
    {
        try
        {
            var externalRequest = request.Adapt<PreOrderRequestDto>();
            var externalResponse = await _balanceApiClient.PreOrderAsync(externalRequest);

            var response = new CreateOrderResponse
            {
                PreOrder = externalResponse.PreOrder.Adapt<PreOrderDto>(),
                UpdatedBalance = externalResponse.UpdatedBalance.Adapt<UpdatedBalanceDto>()
            };

            return ServiceResult<CreateOrderResponse>.Success(response);
        }
        catch (Exception)
        {
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
            return ServiceResult<CompleteOrderResponse>.Failure("COMPLETE_ORDER_FAILED", "Failed to complete the order.");
        }
    }
}