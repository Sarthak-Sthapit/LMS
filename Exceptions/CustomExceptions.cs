using System;

namespace RestAPI.Exceptions
{
 
    // Base Application Exception
    // All custom exceptions inherit from this

    public abstract class AppException : Exception
    {
        public int StatusCode { get; }
        public string ErrorCode { get; }

        protected AppException(string message, int statusCode, string errorCode) 
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }


    // Validation Exception - For input validation failures
    // HTTP 400 Bad Request
   
    public class ValidationException : AppException
    {
        public Dictionary<string, string[]> Errors { get; }

        public ValidationException(string message, Dictionary<string, string[]>? errors = null)
            : base(message, 400, "VALIDATION_ERROR")
        {
            Errors = errors ?? new Dictionary<string, string[]>();
        }

        public ValidationException(string field, string errorMessage)
            : base("Validation failed", 400, "VALIDATION_ERROR")
        {
            Errors = new Dictionary<string, string[]>
            {
                { field, new[] { errorMessage } }
            };
        }
    }

    
    // Not Found Exception - For resource not found scenarios
    // HTTP 404 Not Found
    
    public class NotFoundException : AppException
    {
        public NotFoundException(string resourceName, object key)
            : base($"{resourceName} with id '{key}' was not found.", 404, "NOT_FOUND")
        {
        }

        public NotFoundException(string message)
            : base(message, 404, "NOT_FOUND")
        {
        }
    }

 
    // Conflict Exception - For duplicate/conflict scenarios
    // HTTP 409 Conflict
 
    public class ConflictException : AppException
    {
        public ConflictException(string message)
            : base(message, 409, "CONFLICT")
        {
        }

        public ConflictException(string resourceName, object key)
            : base($"{resourceName} with id '{key}' already exists.", 409, "CONFLICT")
        {
        }
    }


    // Unauthorized Exception - For authentication failures
    // HTTP 401 Unauthorized

    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message = "Authentication failed. Invalid credentials.")
            : base(message, 401, "UNAUTHORIZED")
        {
        }
    }

  
    // Forbidden Exception - For authorization failures
    // HTTP 403 Forbidden
   
    public class ForbiddenException : AppException
    {
        public ForbiddenException(string message = "Access denied. Insufficient permissions.")
            : base(message, 403, "FORBIDDEN")
        {
        }
    }


    // Business Rule Exception - For business logic violations
    // HTTP 422 Unprocessable Entity

    public class BusinessRuleException : AppException
    {
        public BusinessRuleException(string message)
            : base(message, 422, "BUSINESS_RULE_VIOLATION")
        {
        }
    }

 
    //Internal Server Exception - For unexpected server errors
    // HTTP 500 Internal Server Error
 
    public class InternalServerException : AppException
    {
        public InternalServerException(string message = "An internal server error occurred.")
            : base(message, 500, "INTERNAL_SERVER_ERROR")
        {
        }

        public InternalServerException(string message, Exception innerException)
            : base(message, 500, "INTERNAL_SERVER_ERROR")
        {
        }
    }
}