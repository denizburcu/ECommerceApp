namespace ECommerceApp.Domain.DTOs.External;

public class CompleteOrderResponseDto
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public CompleteOrderDataDto Data { get; set; } = new();
}