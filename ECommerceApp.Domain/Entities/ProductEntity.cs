namespace ECommerceApp.Domain.Entities;

/// <summary>
/// Product Entity
/// </summary>
public class ProductEntity
{
    /// <summary>
    /// Unique identifier of the product.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the product.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of the product.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Currency type of the product price (e.g. USD, EUR).
    /// </summary>
    public string Currency { get; set; }

    /// <summary>
    /// Product category (e.g. electronics, clothing).
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// Available stock quantity of the product.
    /// </summary>
    public int Stock { get; set; }
}