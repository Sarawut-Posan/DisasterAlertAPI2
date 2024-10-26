using Microsoft.AspNetCore.Http;

namespace Application.Exceptions;

public class ValidationException : ApiException
{
    public ValidationException(string message) 
        : base(message, StatusCodes.Status400BadRequest)
    {
    }
}