using Microsoft.AspNetCore.Http;

namespace Application.Exceptions;

public class ExternalApiException : ApiException
{
    public ExternalApiException(string message, Exception exception)
        : base(message, StatusCodes.Status503ServiceUnavailable)
    {
    }
}