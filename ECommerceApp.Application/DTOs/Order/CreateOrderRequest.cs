namespace ECommerceApp.Application.DTOs.Order;

public class CreateOrderRequest
{
    /// <summary>
    /// Product Id
    /// </summary>
    public string ProductId  { get; set; }
    
    /// <summary>
    /// Unique identifier for the order.
    /// </summary>
    public string OrderId { get; set; }

    /// <summary>
    /// Amount to be blocked from the balance.
    /// </summary>
    public decimal Amount { get; set; }
}