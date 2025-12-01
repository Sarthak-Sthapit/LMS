

using System;
using System.Collections.Generic;

namespace RestAPI.DTOs
{

    // Standardized Error Response
    // Sent to clients for all errors

    public class ErrorResponse
    {
        
        // User-friendly error message
        
        public string Message { get; set; } = string.Empty;

        
        // HTTP Status Code
        
        public int StatusCode { get; set; }

        
        // Error code for client-side handling
        
        public string ErrorCode { get; set; } = string.Empty;

        
        // Timestamp when error occurred
        
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        
        // Request path where error occurred
        
        public string? Path { get; set; }

        
        // Validation errors (field-specific)
        
        public Dictionary<string, string[]>? Errors { get; set; }

        
        // Stack trace 
        
        public string? StackTrace { get; set; }

        
        // Inner exception details 
        
        public string? InnerException { get; set; }
    }

    
    // Success Response wrapper
    // For consistent API responses
    

    public class SuccessResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    
    // Paginated Response wrapper
    

    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}