namespace ECommerceApp.Application.Common;

/// <summary>
/// Common error codes used by the application.
/// </summary>
public static class ServiceErrorCodes
{
    public const string NotFound = "NOT_FOUND";
    public const string ValidationError = "VALIDATION_ERROR";
    public const string Unauthorized = "UNAUTHORIZED";
    public const string CacheError = "CACHE_ERROR";
    public const string DatabaseError = "DATABASE_ERROR";
    public const string ExternalServiceError = "EXTERNAL_SERVICE_ERROR";
    public const string UnknownError = "UNKNOWN_ERROR";
    public const string UnexpectedError = "UNEXPECTED_ERROR";

}