using System.Net.Http.Json;
using System.Text.Json;
using ECommerceApp.Domain.DTOs.External;
using ECommerceApp.Infrastructure.Configurations;
using ECommerceApp.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ECommerceApp.Infrastructure.Services;

public class BalanceApiClient : IBalanceApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogService _logService;

    public BalanceApiClient(HttpClient httpClient, ILogService logService)
    {
        _httpClient = httpClient;
        _logService = logService;
    }
    
    /// <summary>
    /// Get Product Async
    /// </summary>
    public async Task<List<ProductResponseDto>> GetProductsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/products");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ProductListResponseDto>();
            return result.Data;
        }
        catch (Exception e)
        {
            _logService.Error("BalanceApiClient get products exception", e);

            return null;
        }
    }

    /// <summary>
    /// Pre Order Async
    /// </summary>
    public async Task<PreOrderResponseDto> PreOrderAsync(PreOrderRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/balance/preorder", request);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PreOrderResponseDto>();
        return result!;
    }
    
    // <summary>
    /// Sends a request to complete a pre-order.
    /// </summary>
    public async Task<CompleteOrderResponseDto> CompleteOrderAsync(CompleteOrderRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/balance/complete", request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<CompleteOrderResponseDto>();
        return result!;
    }
}