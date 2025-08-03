namespace ECommerceApp.Domain.DTOs.External;

public class ProductListResponseDto
{
    public bool Success { get; set; }
    public List<ProductResponseDto> Data { get; set; }
}

public class ProductResponseDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public string Category { get; set; }
    public int Stock { get; set; }
}