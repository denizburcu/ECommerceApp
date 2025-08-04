namespace ECommerceApp.Domain.DTOs.External;

public class PreOrderResponseDataDto
{
    /// <summary>
    /// The details of the created pre-order.
    /// </summary>
    public PreOrderDto PreOrder { get; set; }

    /// <summary>
    /// The updated balance after the pre-order is created.
    /// </summary>
    public UpdatedBalanceDto UpdatedBalance { get; set; }
}