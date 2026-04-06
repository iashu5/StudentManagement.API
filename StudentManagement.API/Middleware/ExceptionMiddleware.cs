using StudentManagementSystem.Models;
using System.Text.Json;

namespace StudentManagementSystem.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;  // Changed to Microsoft ILogger

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)  // Changed
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
                _logger.LogError(ex, "An error occurred");  // Changed to LogError
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