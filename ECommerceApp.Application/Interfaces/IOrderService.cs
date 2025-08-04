using ECommerceApp.Domain.DTOs.External;

namespace ECommerceApp.Application.Interfaces;

using ECommerceApp.Application.Common;
using ECommerceApp.Application.DTOs.Order;
public interface IOrderService
{
    Task<ServiceResult<bool>> EnqueueOrderAsync(CreateOrderRequest request);

    /// <summary>
    /// Create order
    /// </summary>
    Task<ServiceResult<CreateOrderResponse>> CreateOrderAsync(CreateOrderRequest request);
    
    /// <summary>
    /// Completes an existing pre-order and finalizes the payment.
    /// </summary>
    Task<ServiceResult<CompleteOrderResponse>> CompleteOrderAsync(string orderId);
}