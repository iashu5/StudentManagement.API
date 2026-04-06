using StudentManagementSystem.Models;
using System.Text.Json;

namespace StudentManagementSystem.Middleware
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");  
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            var response = ApiResponse<object>.Fail(ex.Message);
            context.Response.StatusCode = ex is InvalidOperationException ? 400 : 500;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}