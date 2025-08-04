namespace ECommerceApp.Application.Common;

/// <summary>
/// Represents an error occurred during a service operation.
/// </summary>
public class ServiceError
{
    /// <summary>
    /// A machine-readable error code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// A human-readable error message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// ServiceError.
    /// </summary>
    public ServiceError(string code, string message)
    {
        Code = code;
        Message = message;
    }
}