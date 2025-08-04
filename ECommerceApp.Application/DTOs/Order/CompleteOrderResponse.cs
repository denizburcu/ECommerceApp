namespace ECommerceApp.Application.DTOs.Order;

using ECommerceApp.Domain.DTOs.External;

public class CompleteOrderResponse
{
        public PreOrderDto? PreOrder { get; set; }
        public UpdatedBalanceDto? UpdatedBalance { get; set; }
    
}