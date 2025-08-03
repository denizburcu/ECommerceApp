namespace ECommerceApp.Domain.DTOs.External;

public class PreOrderRequestDto
{
    public List<string> ProductIds { get; set; } = new();
}

public class PreOrderResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class CompleteOrderRequestDto
{
    public string OrderId { get; set; } = string.Empty;
}

public class CompleteOrderResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
