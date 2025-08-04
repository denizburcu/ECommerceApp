namespace ECommerceApp.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using MassTransit;
using ECommerceApp.Application.DTOs.Order;
using ECommerceApp.Application.Interfaces;
using ECommerceApp.Shared.Commands;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrdersController(IOrderService orderService,IPublishEndpoint publishEndpoint)
    {
        _orderService = orderService;
        _publishEndpoint = publishEndpoint;
    }

    /// <summary>
    /// Creates a new order by reserving the balance.
    /// </summary>
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var result = await _orderService.EnqueueOrderAsync(request);

        return Ok(result); 
    }
    
    /// <summary>
    /// Completes an order and finalizes the payment using Balance API.
    /// </summary>
    [HttpPost("{id}/complete")]
    public async Task<IActionResult> CompleteOrder(string id)
    {
        var result = await _orderService.CompleteOrderAsync(id);

        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                success = false,
                error = new
                {
                    code = result.Error?.Code,
                    message = result.Error?.Message
                }
            });
        }
        return Ok(result.Data);
    }
}