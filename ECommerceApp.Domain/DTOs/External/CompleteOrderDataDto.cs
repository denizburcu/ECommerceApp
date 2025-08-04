namespace ECommerceApp.Domain.DTOs.External;

public class CompleteOrderDataDto
{
    public PreOrderDto Order { get; set; } = new();
    
    public UpdatedBalanceDto UpdatedBalance { get; set; } = new();
}