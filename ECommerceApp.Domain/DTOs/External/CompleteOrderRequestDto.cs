namespace ECommerceApp.Domain.DTOs.External;

public class CompleteOrderRequestDto
{
    /// <summary>
    /// Unique identifier of the order to be completed.
    /// </summary>
    public string OrderId { get; set; } = string.Empty;
}