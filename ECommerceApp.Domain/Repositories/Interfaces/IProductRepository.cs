namespace ECommerceApp.Domain.Repositories;

using ECommerceApp.Domain.Entities;

/// <summary>
/// Defines contract for product data access operations.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Retrieves all products from the data source.
    /// </summary>
    Task<IEnumerable<ProductEntity>> GetAllAsync();

    /// <summary>
    /// Retrieves a product by its external identifier.
    /// </summary>
    Task<ProductEntity?> GetByExternalIdAsync(string externalId);

    /// <summary>
    /// Retrieves a single product by its unique identifier.
    /// </summary>
    Task<ProductEntity?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new product to the data source.
    /// </summary>
    Task AddAsync(ProductEntity product);

    /// <summary>
    /// Updates an existing product in the data source.
    /// </summary>
    void Update(ProductEntity product);

    /// <summary>
    /// Deletes a product from the data source.
    /// </summary>
    void Delete(ProductEntity product);

    /// <summary>
    /// Persists all changes made in the context to the data source.
    /// </summary>
    Task<bool> SaveChangesAsync();
}