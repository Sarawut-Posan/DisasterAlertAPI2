using Microsoft.AspNetCore.Http;

namespace Application.Exceptions;

public class DataNotFoundException : ApiException
{
    public DataNotFoundException(string message) 
        : base(message, StatusCodes.Status404NotFound)
    {
    }
}