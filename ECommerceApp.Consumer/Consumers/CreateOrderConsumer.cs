namespace ECommerceApp.Consumer.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using ECommerceApp.Application.DTOs.Order;
using ECommerceApp.Application.Interfaces;
using ECommerceApp.Shared.Commands;

public class CreateOrderConsumer : IConsumer<CreateOrderCommand>
{
    private readonly IOrderService _orderService;
    private readonly ILogger<CreateOrderConsumer> _logger;

    public CreateOrderConsumer(IOrderService orderService, ILogger<CreateOrderConsumer> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CreateOrderCommand> context)
    {
        var message = context.Message;
        _logger.LogInformation("Received order: {OrderId}, Amount: {Amount}", message.OrderId, message.Amount);

        var request = new CreateOrderRequest
        {
            OrderId = message.OrderId,
            ProductId = message.ProductId,
            Amount = message.Amount
        };

        var result = await _orderService.CreateOrderAsync(request);

        if (result.Succeeded)
        {
            _logger.LogInformation("Order {OrderId} processed successfully", message.OrderId);
        }
        else
        {
            _logger.LogError("Order {OrderId} failed: {Error}", message.OrderId, result.Error?.Message);
        }
    }
}