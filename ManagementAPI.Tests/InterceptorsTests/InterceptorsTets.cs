using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Moq;
using ManagementAPI.DTO;
using ManagementAPI.Interceptors;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace ManagementAPI.Tests.InterceptorsTests
{
    public class ExceptionFilterTests
    {
        [Fact]
        public void OnException_ShouldSetInternalServerErrorResult()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(
                httpContext,
                new RouteData(),
                new ControllerActionDescriptor());

            var context = new ExceptionContext(actionContext, new List<IFilterMetadata>())
            {
                Exception = new Exception("Test exception")
            };

            var filter = new ExceptionFilter();

            // Act
            filter.OnException(context);

            // Assert
            var result = Assert.IsType<ObjectResult>(context.Result);
            Assert.Equal(500, result.StatusCode);

            var value = Assert.IsType<InternalServerErrorDto>(result.Value);
            Assert.Equal(500, value.status);
            Assert.Equal("An unexpected error occurred. Please try again later.", value.message);
        }
    }

    public class NotFoundFilterTests
    {
        private static ResultExecutingContext CreateContext(IActionResult result)
        {
            var httpContext = new DefaultHttpContext();
            var routeData = new RouteData();
            var actionDescriptor = new ControllerActionDescriptor();
            var actionContext = new ActionContext(httpContext, routeData, actionDescriptor);

            return new ResultExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                result,
                controller: null);
        }

        [Fact]
        public async Task OnResultExecutionAsync_WithStringMessage_ShouldWrapInJsonResult()
        {
            var context = CreateContext(new NotFoundObjectResult("Item not found"));
            var next = Mock.Of<ResultExecutionDelegate>();
            var filter = new NotFoundFilter();

            await filter.OnResultExecutionAsync(context, next);

            var result = Assert.IsType<JsonResult>(context.Result);
            Assert.Equal(404, result.StatusCode);

            var dict = result.Value!.GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(result.Value));

            Assert.Equal("Item not found", dict["message"]);
            Assert.Equal("Not Found", dict["error"]);
        }

        [Fact]
        public async Task OnResultExecutionAsync_WithAnonymousObjectContainingMessage_ShouldExtractMessage()
        {
            var context = CreateContext(new NotFoundObjectResult(new { message = "custom not found" }));
            var next = Mock.Of<ResultExecutionDelegate>();
            var filter = new NotFoundFilter();

            await filter.OnResultExecutionAsync(context, next);

            var result = Assert.IsType<JsonResult>(context.Result);
            Assert.Equal(404, result.StatusCode);

            var dict = result.Value!.GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(result.Value));

            Assert.Equal("custom not found", dict["message"]);
            Assert.Equal("Not Found", dict["error"]);
        }

        [Fact]
        public async Task OnResultExecutionAsync_WithNoMessage_ShouldUseDefault()
        {
            var context = CreateContext(new NotFoundObjectResult(12345));
            var next = Mock.Of<ResultExecutionDelegate>();
            var filter = new NotFoundFilter();

            await filter.OnResultExecutionAsync(context, next);

            var result = Assert.IsType<JsonResult>(context.Result);
            Assert.Equal(404, result.StatusCode);

            var dict = result.Value!.GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(result.Value));

            Assert.Equal("Resource not found", dict["message"]);
            Assert.Equal("Not Found", dict["error"]);
        }
    }
}
