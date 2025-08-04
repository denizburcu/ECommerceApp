using ECommerceApp.Application.DTOs.Order;
using ECommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Creates a new order by reserving the balance.
    /// </summary>
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        var result = await _orderService.CreateOrderAsync(request);

        if (result.Succeeded)
        {
            return Ok(new
            {
                success = true,
                message = "Pre-order created successfully",
                data = result.Data
            });
        }
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