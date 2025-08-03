using ECommerceApp.Application.DTOs.Product;
using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Application.Interfaces;

/// <summary>
/// Defines operations related to Product business logic.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Retrieves all products.
    /// </summary>
    Task<IEnumerable<ProductDto>> GetAllAsync();

    /// <summary>
    /// Retrieves a product by its unique identifier.
    /// </summary>
    Task<ProductDto> GetByIdAsync(Guid id);

    /// <summary>
    /// Creates a new product.
    /// </summary>
    Task<ProductDto> CreateAsync(CreateProductRequest dto);

    /// <summary>
    /// Updates an existing product by ID.
    /// </summary>
    Task<bool> UpdateAsync(Guid id, UpdateProductRequest dto);

    /// <summary>
    /// Deletes a product by its ID.
    /// </summary>
    Task<bool> DeleteAsync(Guid id);
}