using System.Text.Json;
using EventApiDemo.Models;

namespace EventApiDemo.Middleware
{


    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Middlewaring: An error occurred while processing your request.");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            return context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Middlewaring: An error occurred while processing your request. Please try again later."
            }));
        }
    }
}