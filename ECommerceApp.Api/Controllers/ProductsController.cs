using ECommerceApp.Application.DTOs.Product;
using ECommerceApp.Application.Interfaces;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Infrastructure.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Api.Controllers;

/// <summary>
/// Handles product-related API requests.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    
    /// <summary>
    /// Returns all products.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(new { success = true, data = products });
    }
}
