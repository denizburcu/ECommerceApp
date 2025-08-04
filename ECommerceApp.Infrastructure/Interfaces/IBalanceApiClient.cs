namespace ECommerceApp.Infrastructure.Interfaces;
using ECommerceApp.Domain.DTOs.External;

public interface IBalanceApiClient
{
    /// <summary>
    /// GetProductsAsync
    /// </summary>
    Task<List<ProductResponseDto>> GetProductsAsync();
    
    /// <summary>
    /// Sends a pre-order request to the balance API.
    /// </summary>
    Task<PreOrderResponseDto> PreOrderAsync(PreOrderRequestDto request);
    
    /// <summary>
    /// Sends a request to complete an order by deducting the blocked amount from total balance.
    /// </summary>
    Task<CompleteOrderResponseDto> CompleteOrderAsync(CompleteOrderRequestDto request);}
