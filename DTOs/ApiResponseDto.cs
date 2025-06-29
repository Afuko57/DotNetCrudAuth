using System.Text.Json.Serialization;

namespace MyApiTest.DTOs
{
    public record ApiResponseDto<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; init; }

        [JsonPropertyName("message")]
        public required string Message { get; init; }

        [JsonPropertyName("code")]
        public required string Code { get; init; }

        [JsonPropertyName("data")]
        public T? Data { get; init; }

        [JsonPropertyName("errors")]
        public List<string>? Errors { get; init; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; init; } = DateTime.UtcNow;

        public static ApiResponseDto<T> SuccessResponse(T data, string message = "สำเร็จ", string code = "SUCCESS")
        {
            return new ApiResponseDto<T>
            {
                Success = true,
                Message = message,
                Code = code,
                Data = data
            };
        }

        public static ApiResponseDto<T> ErrorResponse(string message, string code = "ERROR", List<string>? errors = null)
        {
            return new ApiResponseDto<T>
            {
                Success = false,
                Message = message,
                Code = code,
                Errors = errors
            };
        }

        public static ApiResponseDto<T> ValidationErrorResponse(List<string> errors, string message = "ข้อมูลไม่ถูกต้อง")
        {
            return new ApiResponseDto<T>
            {
                Success = false,
                Message = message,
                Code = "VALIDATION_ERROR",
                Errors = errors
            };
        }
    }

    public record ApiResponseDto : ApiResponseDto<object>
    {
        public static new ApiResponseDto SuccessResponse(string message = "สำเร็จ", string code = "SUCCESS")
        {
            return new ApiResponseDto
            {
                Success = true,
                Message = message,
                Code = code
            };
        }

        public static new ApiResponseDto ErrorResponse(string message, string code = "ERROR", List<string>? errors = null)
        {
            return new ApiResponseDto
            {
                Success = false,
                Message = message,
                Code = code,
                Errors = errors
            };
        }
    }
}
