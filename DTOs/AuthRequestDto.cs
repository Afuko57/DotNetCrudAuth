using System.ComponentModel.DataAnnotations;

namespace MyApiTest.DTOs
{
    public record AuthRequestDto
    {
        [Required(ErrorMessage = "Please enter your username")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Your username must be 3-50 characters long")]
        public string Username { get; init; } = string.Empty;

        [Required(ErrorMessage = "Please enter your password")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Your password must be at least 6 characters long")]
        public string Password { get; init; } = string.Empty;
    }
}
