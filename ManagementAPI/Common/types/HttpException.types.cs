
namespace ManagementAPI.Types;

public class HttpResponseException : Exception
{
    public int StatusCode { get; set; }
    public object? ErrorObject { get; set; }

    public HttpResponseException(int statusCode, object? errorObject = null)
    {
        StatusCode = statusCode;
        ErrorObject = errorObject;
    }
}