using System.Net;
using System.Text.Json;

namespace ManagementAPI.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                await WriteCustomResponseAsync(context, HttpStatusCode.Unauthorized, "Unauthorized access. Please log in to continue.");
            }
            else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
            {
                await WriteCustomResponseAsync(context, HttpStatusCode.Forbidden, "You do not have permission to access this resource.");
            }
        }

        private static async Task WriteCustomResponseAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;

                var result = JsonSerializer.Serialize(new
                {
                    statusCode = (int)statusCode,
                    error = statusCode.ToString(),
                    message
                });

                await context.Response.WriteAsync(result);
            }
        }
    }
}
