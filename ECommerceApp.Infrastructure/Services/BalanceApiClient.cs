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

    public async Task<PreOrderResponseDto> PreOrderAsync(PreOrderRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/preorder", request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PreOrderResponseDto>();
        return result!;
    }

    public async Task<CompleteOrderResponseDto> CompleteOrderAsync(CompleteOrderRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/complete", request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<CompleteOrderResponseDto>();
        return result!;
    }
}