namespace ECommerceApp.Application.Common;

/// <summary>
/// Represents the outcome of a service operation.
/// </summary>
public class ServiceResult<T>
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool Succeeded { get; }

    /// <summary>
    /// The returned data (if successful).
    /// </summary>
    public T? Data { get; }

    /// <summary>
    /// The error details (if failed).
    /// </summary>
    public ServiceError? Error { get; }

    private ServiceResult(bool succeeded, T? data = default, ServiceError? error = null)
    {
        Succeeded = succeeded;
        Data = data;
        Error = error;
    }

    /// <summary>
    /// Creates a success result.
    /// </summary>
    public static ServiceResult<T> Success(T data) => new(true, data);

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    public static ServiceResult<T> Failure(string code, string message) =>
        new(false, default, new ServiceError(code, message));

    /// <summary>
    /// Creates a failure result from an error object.
    /// </summary>
    public static ServiceResult<T> Failure(ServiceError error) =>
        new(false, default, error);
}