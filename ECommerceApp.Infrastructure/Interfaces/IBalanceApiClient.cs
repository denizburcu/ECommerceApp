namespace ECommerceApp.Infrastructure.Interfaces;
using ECommerceApp.Domain.DTOs.External;

public interface IBalanceApiClient
{
    Task<List<ProductResponseDto>> GetProductsAsync();
    Task<PreOrderResponseDto> PreOrderAsync(PreOrderRequestDto request);
    Task<CompleteOrderResponseDto> CompleteOrderAsync(CompleteOrderRequestDto request);
}
