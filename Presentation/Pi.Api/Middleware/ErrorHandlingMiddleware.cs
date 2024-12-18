using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pi.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                // Add more exception types as needed
                _ => (int)HttpStatusCode.InternalServerError
            };

            var response = new
            {
                Status = statusCode,
                Message = GetUserFriendlyMessage(exception),
                DetailedMessage = exception.Message,
#if DEBUG
                StackTrace = exception.StackTrace
#endif
            };

            context.Response.StatusCode = response.Status;

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonOptions));
        }

        private string GetUserFriendlyMessage(Exception exception) => exception switch
        {
            KeyNotFoundException => "The requested resource was not found.",
            UnauthorizedAccessException => "You are not authorized to access this resource.",
            ArgumentException => "Invalid input provided.",
            _ => "An unexpected error occurred."
        };
    }

    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
