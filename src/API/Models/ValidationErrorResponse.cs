namespace API.Models;

public class ValidationErrorResponse : ErrorResponse
{
    public IDictionary<string, string[]> Errors { get; set; }
}