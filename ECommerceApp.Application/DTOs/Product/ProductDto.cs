namespace ECommerceApp.Application.DTOs.Product;

/// <summary>
/// Represents a product to be returned in API responses.
/// </summary>
public class ProductDto
{
    /// <summary>
    /// Unique identifier of the product.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the product.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Description of the product.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Currency type (e.g., "USD", "TRY").
    /// </summary>
    public string Currency { get; set; } = null!;

    /// <summary>
    /// Product category.
    /// </summary>
    public string Category { get; set; } = null!;

    /// <summary>
    /// Stock quantity.
    /// </summary>
    public int Stock { get; set; }
}