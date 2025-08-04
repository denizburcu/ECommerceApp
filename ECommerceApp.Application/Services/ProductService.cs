namespace ECommerceApp.Application.Services;
using ECommerceApp.Application.DTOs.Product;
using ECommerceApp.Application.Interfaces;
using ECommerceApp.Infrastructure.Interfaces;
using ECommerceApp.Application.DTOs;
using ECommerceApp.Application.Services;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Repositories;
using Mapster;

/// <summary>
/// Provides business logic for managing products.
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogService _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductService"/> class.
    /// </summary>
    public ProductService(IProductRepository productRepository, ICacheService cacheService, ILogService logger)
    {
        _productRepository = productRepository;
        _cacheService = cacheService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all products.
    /// </summary>
    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        const string cacheKey = "products:all";

        try
        {
            var cached = await _cacheService.GetAsync<List<ProductDto>>(cacheKey);
            if (cached is not null)
            {
                _logger.Info($"Fetched {cached.Count} products from Redis cache.");
                return cached;
            }

            _logger.Warn("No product data found in Redis cache. Falling back to DB.");
        }
        catch (Exception ex)
        {
            _logger.Error("Redis read failed. Falling back to DB.", ex);
        }

        var productsFromDb = await _productRepository.GetAllAsync();
        var dtos = productsFromDb.Adapt<List<ProductDto>>();

        try
        {
            await _cacheService.SetAsync(cacheKey, dtos);
            _logger.Info($"Cached {dtos.Count} products to Redis with key '{cacheKey}'.");
        }
        catch (Exception ex)
        {
            _logger.Error("Redis write failed after DB fetch.", ex);
        }
        return dtos;
    }

    /// <summary>
    /// Retrieves a product by its unique identifier.
    /// </summary>
    public async Task<ProductDto> GetByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product.Adapt<ProductDto>();
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    public async Task<ProductDto> CreateAsync(CreateProductRequest dto)
    {
        var product = dto.Adapt<ProductEntity>();
        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();
        return product.Adapt<ProductDto>();
    }

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    public async Task<bool> UpdateAsync(Guid id, UpdateProductRequest dto)
    {
        var existing = await _productRepository.GetByIdAsync(id);
        if (existing is null) return false;

        dto.Adapt(existing);
        _productRepository.Update(existing);
        await _productRepository.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Deletes a product by its ID.
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _productRepository.GetByIdAsync(id);
        if (existing is null) return false;

        _productRepository.Delete(existing);
        return await _productRepository.SaveChangesAsync();
    }
}
