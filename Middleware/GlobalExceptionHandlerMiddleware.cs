using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using RestAPI.DTOs;
using RestAPI.Exceptions;

namespace RestAPI.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public GlobalExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlerMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception,
                    "An unhandled exception occurred: {Message}",
                    exception.Message);

                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var errorResponse = exception switch
            {
                
                ValidationException validationEx => CreateValidationErrorResponse(
                    validationEx.Message,
                    validationEx.Errors,
                    context.Request.Path,
                    validationEx
                ),

                //  custom AppExceptions (NotFound, Conflict, Forbidden, etc.)
                AppException appEx => CreateErrorResponse(
                    appEx.Message,
                    appEx.StatusCode,
                    appEx.ErrorCode,
                    context.Request.Path,
                    appEx
                ),

                //  Fallback for unexpected exceptions
                _ => CreateErrorResponse(
                    "An unexpected error occurred. Please try again later.",
                    StatusCodes.Status500InternalServerError,
                    "INTERNAL_SERVER_ERROR",
                    context.Request.Path,
                    exception
                )
            };

            context.Response.StatusCode = errorResponse.StatusCode;

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = _environment.IsDevelopment()
            };

            var json = JsonSerializer.Serialize(errorResponse, jsonOptions);
            await context.Response.WriteAsync(json);
        }

        private ErrorResponse CreateErrorResponse(
            string message,
            int statusCode,
            string errorCode,
            string path,
            Exception exception)
        {
            var errorResponse = new ErrorResponse
            {
                Message = message,
                StatusCode = statusCode,
                ErrorCode = errorCode,
                Path = path,
                Timestamp = DateTime.UtcNow
            };

            //  sensitive details only in Development
            if (_environment.IsDevelopment())
            {
                errorResponse.StackTrace = exception.StackTrace;
                errorResponse.InnerException = exception.InnerException?.Message;
            }

            return errorResponse;
        }

        private ErrorResponse CreateValidationErrorResponse(
            string message,
            Dictionary<string, string[]> errors,
            string path,
            Exception exception)
        {
            var response = CreateErrorResponse(
                message,
                StatusCodes.Status400BadRequest,
                "VALIDATION_ERROR",
                path,
                exception
            );

            response.Errors = errors;
            return response;
        }
    }

    public static class GlobalExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(
            this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}
