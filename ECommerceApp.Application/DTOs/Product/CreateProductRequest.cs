namespace ECommerceApp.Application.DTOs.Product;

/// <summary>
/// Represents the request payload for creating a new product.
/// </summary>
public class CreateProductRequest
{
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