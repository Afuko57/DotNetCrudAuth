using System.Text.Json.Serialization;

namespace MyApiTest.DTOs
{
    public record UserDto
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("username")]
        public string Username { get; init; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; init; }

        public UserDto(int id, string username, DateTime? createdAt = null)
        {
            Id = id;
            Username = username;
            CreatedAt = createdAt;
        }
    }
}
