using System.Text.Json.Serialization;

namespace MyApiTest.DTOs
{
    public record AuthResponseDto
    {
        [JsonPropertyName("success")]
        public bool Success { get; init; }

        [JsonPropertyName("message")]
        public string Message { get; init; }

        [JsonPropertyName("code")]
        public string Code { get; init; }

        [JsonPropertyName("user")]
        public UserDto? User { get; init; }

        [JsonPropertyName("token")]
        public string? Token { get; init; }

        public AuthResponseDto(bool success, string message, string code = "")
        {
            Success = success;
            Message = message;
            Code = code;
        }

        public AuthResponseDto(bool success, string message, UserDto user, string code = "")
        {
            Success = success;
            Message = message;
            User = user;
            Code = code;
        }
    }
}
