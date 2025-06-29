using MyApiTest.Models;
using MyApiTest.DTOs;

namespace MyApiTest.Extensions
{
    public static class MappingExtensions
    {
        public static UserDto ToDto(this User user)
        {
            return new UserDto(
                id: user.Id,
                username: user.Username,
                createdAt: DateTime.UtcNow
            );
        }

        public static User ToUserModel(this AuthRequestDto request, string hashedPassword)
        {
            return new User
            {
                Username = request.Username,
                PasswordHash = hashedPassword
            };
        }
    }
}