namespace ECommerceApp.Domain.DTOs.External;

/// <summary>
/// Represents the top-level response from the PreOrder API.
/// </summary>
public class PreOrderResponseDto
{
    public bool Success { get; set; }

    public string Message { get; set; }

    public PreOrderResponseDataDto Data { get; set; }
}