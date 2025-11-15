using HRMS.Models.Enums;

namespace HRMS.ViewModels
{
    public class ResponseViewModel<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public ErrorCode ErrorCode { get; set; }

        // Success with data
        public static ResponseViewModel<T> Success(T data, string message = "Operation completed successfully")
        {
            return new ResponseViewModel<T>
            {
                Data = data,
                IsSuccess = true,
                Message = message,
                ErrorCode = ErrorCode.NoError
            };
        }

        // Success without data
        public static ResponseViewModel<T> Success(string message = "Operation completed successfully")
        {
            return new ResponseViewModel<T>
            {
                Data = default(T),
                IsSuccess = true,
                Message = message,
                ErrorCode = ErrorCode.NoError
            };
        }

        // Failure with error code
        public static ResponseViewModel<T> Failure(string message, ErrorCode errorCode = ErrorCode.GeneralError)
        {
            return new ResponseViewModel<T>
            {
                Data = default(T),
                IsSuccess = false,
                Message = message,
                ErrorCode = errorCode
            };
        }

        // Not Found
        public static ResponseViewModel<T> NotFound(string message = "Resource not found")
        {
            return new ResponseViewModel<T>
            {
                Data = default(T),
                IsSuccess = false,
                Message = message,
                ErrorCode = ErrorCode.NotFound
            };
        }

        // Validation Error
        public static ResponseViewModel<T> ValidationError(string message = "Validation failed")
        {
            return new ResponseViewModel<T>
            {
                Data = default(T),
                IsSuccess = false,
                Message = message,
                ErrorCode = ErrorCode.ValidationError
            };
        }

        // Unauthorized
        public static ResponseViewModel<T> Unauthorized(string message = "Unauthorized access")
        {
            return new ResponseViewModel<T>
            {
                Data = default(T),
                IsSuccess = false,
                Message = message,
                ErrorCode = ErrorCode.Unauthorized
            };
        }
    }
}