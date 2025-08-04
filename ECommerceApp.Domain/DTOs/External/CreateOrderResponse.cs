namespace ECommerceApp.Domain.DTOs.External;

public class CreateOrderResponse
{
    /// <summary>
    /// Details of the created pre-order.
    /// </summary>
    public PreOrderDto PreOrder { get; set; }

    /// <summary>
    /// Updated balance information after blocking the amount.
    /// </summary>
    public UpdatedBalanceDto UpdatedBalance { get; set; }
}