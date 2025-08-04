namespace ECommerceApp.Application.BackgroundServices;

using ECommerceApp.Infrastructure.Interfaces;
using ECommerceApp.Domain.Repositories;
using ECommerceApp.Domain.Entities;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

/// <summary>
/// Periodically syncs product data from external Balance API into local database.
/// </summary>
public class ProductSyncBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogService _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);

    public ProductSyncBackgroundService(IServiceProvider serviceProvider, ILogService logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.Info("ProductSyncBackgroundService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.Info($"Starting product sync at {DateTime.UtcNow.ToLongDateString()}");

            try
            {
                using var scope = _serviceProvider.CreateScope();

                var apiClient = scope.ServiceProvider.GetRequiredService<IBalanceApiClient>();
                var repository = scope.ServiceProvider.GetRequiredService<IProductRepository>();

                var externalProducts = await apiClient.GetProductsAsync();
                _logger.Info($"Fetched {externalProducts.Count} products from external API.");

                foreach (var extProduct in externalProducts)
                {
                    var product = new ProductEntity
                    {
                        Id = Guid.NewGuid(),
                        ExternalProductId = extProduct.Id,
                        Name = extProduct.Name,
                        Description = extProduct.Description,
                        Price = extProduct.Price,
                        Currency = extProduct.Currency,
                        Category = extProduct.Category,
                        Stock = extProduct.Stock,
                    };

                    var existing = await repository.GetByExternalIdAsync(extProduct.Id);

                    if (existing is null)
                    {
                        await repository.AddAsync(product);
                        _logger.Info($"Added new product: {product.Id} - {product.Name}");
                    }
                    else
                    {
                        repository.Update(product);
                        _logger.Info($"Updated existing product: {product.Id} - {product.Name}");
                    }
                }

                await repository.SaveChangesAsync();

                _logger.Info($"Product sync completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred during product sync.", ex);
            }
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

                var allProducts = await repository.GetAllAsync();
                const string cacheKey = "products:all";

                await cacheService.SetAsync(cacheKey, allProducts);
                _logger.Info($"[CACHE] Cached {allProducts.Count()} products to Redis with key '{cacheKey}'.");
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred while caching products to Redis.", ex);
            }
            
            await Task.Delay(_interval, stoppingToken);
        }
        _logger.Info("ProductSyncBackgroundService is stopping.");
    }
}