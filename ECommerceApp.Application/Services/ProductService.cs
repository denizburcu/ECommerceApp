using ECommerceApp.Application.DTOs.Product;
using ECommerceApp.Application.Interfaces;

namespace ECommerceApp.Application.Services;
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

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductService"/> class.
    /// </summary>
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Retrieves all products.
    /// </summary>
    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Adapt<IEnumerable<ProductDto>>();
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
