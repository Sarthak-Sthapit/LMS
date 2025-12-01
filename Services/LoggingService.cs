using System;
using Microsoft.Extensions.Logging;
using RestAPI.Exceptions;

namespace RestAPI.Services
{
    
    // Logging Service Interface
    
    public interface ILoggingService
    {
        void LogInformation(string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogError(Exception exception, string message, params object[] args);
        void LogError(string message, params object[] args);
        void LogDebug(string message, params object[] args);
        void LogCritical(Exception exception, string message, params object[] args);
    }

    
    // Logging Service Implementation
    // Wraps ILogger for centralized logging with additional features
    // Design Pattern: Adapter Pattern
    
    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        
        // Log informational message
        
        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
            
        }

        
        // Log warning message
        
        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
          
        }

        
        // Log error with exception
        
        public void LogError(Exception exception, string message, params object[] args)
        {
            _logger.LogError(exception, message, args);
            
            // Log additional context
            LogExceptionDetails(exception);
            
            
        }

        
        // Log error without exception
        
        public void LogError(string message, params object[] args)
        {
            _logger.LogError(message, args);
        }

        
        // Log debug message (only in Development)
        
        public void LogDebug(string message, params object[] args)
        {
            _logger.LogDebug(message, args);
        }

        
        // Log critical error
        
        public void LogCritical(Exception exception, string message, params object[] args)
        {
            _logger.LogCritical(exception, message, args);
            
            LogExceptionDetails(exception);
        
        }

        
        // Log detailed exception information
        
        private void LogExceptionDetails(Exception exception)
        {
            _logger.LogError("Exception Type: {ExceptionType}", exception.GetType().Name);
            _logger.LogError("Exception Message: {Message}", exception.Message);
            _logger.LogError("Stack Trace: {StackTrace}", exception.StackTrace);
            
            if (exception.InnerException != null)
            {
                _logger.LogError("Inner Exception: {InnerException}", 
                    exception.InnerException.Message);
            }

            // Log custom exception properties if available
            if (exception is AppException appEx)
            {
                _logger.LogError("Error Code: {ErrorCode}", appEx.ErrorCode);
                _logger.LogError("Status Code: {StatusCode}", appEx.StatusCode);
            }
        }

       
    }

    
    // Structured Log Entry
    // For advanced logging scenarios
    
    public class LogEntry
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Level { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? RequestPath { get; set; }
        public string? ExceptionType { get; set; }
        public Dictionary<string, object>? AdditionalData { get; set; }
    }
}