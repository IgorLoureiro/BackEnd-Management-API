using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ManagementAPI.Interceptors;

public class NotFoundFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is NotFoundObjectResult notFoundResult)
        {
            string customMessage = "Resource not found";

            if (notFoundResult.Value is IDictionary<string, object> dict && dict.ContainsKey("message"))
            {
                customMessage = dict["message"]?.ToString() ?? customMessage;
            }
            else if (notFoundResult.Value is string strValue)
            {
                customMessage = strValue;
            }
            else if (notFoundResult.Value != null)
            {
                /* extract custom message */
                var valueType = notFoundResult.Value.GetType();
                var messageProperty = valueType.GetProperty("message");
                if (messageProperty != null)
                {
                    var msgValue = messageProperty.GetValue(notFoundResult.Value);
                    if (msgValue != null)
                    {
                        customMessage = msgValue.ToString();
                    }
                }
            }

            context.Result = new JsonResult(new
            {
                StatusCode = 404,
                message = customMessage,
                error = "Not Found"
            })
            {
                StatusCode = 404
            };
        }

        await next();
    }
}
