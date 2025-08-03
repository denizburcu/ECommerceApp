using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Repositories;
using ECommerceApp.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Persistence.Repositories;

/// <summary>
/// Provides implementation for product-related data access operations using Entity Framework Core.
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly IDataContext _context;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductRepository"/> class.
    /// </summary>
    public ProductRepository(IDataContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Retrieves all products from the database.
    /// </summary>
    public async Task<IEnumerable<ProductEntity>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    /// <summary>
    /// Retrieves a product by its unique identifier.
    /// </summary>
    public async Task<ProductEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Products.Where(x=>x.Id == id).FirstOrDefaultAsync();
    }
    
    /// <summary>
    /// Retrieves a product by its external identifier.
    /// </summary>
    public async Task<ProductEntity?> GetByExternalIdAsync(string externalId)
    {
        return await _context.Products.Where(x=>x.ExternalProductId == externalId).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Adds a new product to the database.
    /// </summary>
    public async Task AddAsync(ProductEntity product)
    {
        await _context.Products.AddAsync(product);
    }

    /// <summary>
    /// Updates an existing product in the database.
    /// </summary>
    public void Update(ProductEntity product)
    {
        _context.Products.Update(product);
    }

    /// <summary>
    /// Deletes a product from the database.
    /// </summary>
    public void Delete(ProductEntity product)
    {
        _context.Products.Remove(product);
    }

    /// <summary>
    /// Saves all changes made in the context to the database.
    /// </summary>
    public async Task<bool> SaveChangesAsync()
    {
        return await _unitOfWork.SaveAsync() > 0;
    }
}
