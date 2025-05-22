using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Result = new ObjectResult(new
        {
            status = 500,
            message = "An unexpected error occurred. Please try again later."
        })
        {
            StatusCode = 500
        };
        
        Console.WriteLine($"Exception caught: {context.Exception.Message}");
    }
}
