using MyApiTest.Models;
using MyApiTest.Interfaces;
using MyApiTest.DTOs;
using MyApiTest.Extensions;
using System.Security.Cryptography;
using System.Text;

namespace MyApiTest.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AuthResponseDto> RegisterAsync(AuthRequestDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return new AuthResponseDto(
                        success: false,
                        message: "Username and password are required",
                        code: "MISSING_REQUIRED_FIELDS"
                    );
                }

                if (request.Username.Length < 3)
                {
                    return new AuthResponseDto(
                        success: false,
                        message: "Username must be at least 3 characters long",
                        code: "USERNAME_TOO_SHORT"
                    );
                }

                if (request.Password.Length < 6)
                {
                    return new AuthResponseDto(
                        success: false,
                        message: "Password must be at least 6 characters long",
                        code: "PASSWORD_TOO_SHORT"
                    );
                }

                if (await _userRepository.ExistsAsync(request.Username))
                {
                    return new AuthResponseDto(
                        success: false,
                        message: "This username is already taken",
                        code: "USERNAME_EXISTS"
                    );
                }

                var hashedPassword = HashPassword(request.Password);
                var user = request.ToUserModel(hashedPassword);

                await _userRepository.AddAsync(user);

                var userDto = user.ToDto();
                return new AuthResponseDto(
                    success: true,
                    message: "Registration successful",
                    user: userDto,
                    code: "REGISTER_SUCCESS"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RegisterAsync: {ex.Message}");
                return new AuthResponseDto(
                    success: false,
                    message: "Internal server error",
                    code: "INTERNAL_ERROR"
                );
            }
        }

        public async Task<AuthResponseDto> LoginAsync(AuthRequestDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return new AuthResponseDto(
                        success: false,
                        message: "Username and password are required",
                        code: "MISSING_CREDENTIALS"
                    );
                }

                var user = await _userRepository.GetByUsernameAsync(request.Username);
                if (user == null)
                {
                    return new AuthResponseDto(
                        success: false,
                        message: "Invalid username or password",
                        code: "INVALID_CREDENTIALS"
                    );
                }

                var hashedPassword = HashPassword(request.Password);
                if (user.PasswordHash != hashedPassword)
                {
                    return new AuthResponseDto(
                        success: false,
                        message: "Invalid username or password",
                        code: "INVALID_CREDENTIALS"
                    );
                }

                var userDto = user.ToDto();
                return new AuthResponseDto(
                    success: true,
                    message: "Login successful",
                    user: userDto,
                    code: "LOGIN_SUCCESS"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoginAsync: {ex.Message}");
                return new AuthResponseDto(
                    success: false,
                    message: "Internal server error",
                    code: "INTERNAL_ERROR"
                );
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public async Task<bool> Register(string username, string password)
        {
            var request = new AuthRequestDto { Username = username, Password = password };
            var result = await RegisterAsync(request);
            return result.Success;
        }

        public async Task<bool> Login(string username, string password)
        {
            var request = new AuthRequestDto { Username = username, Password = password };
            var result = await LoginAsync(request);
            return result.Success;
        }
    }
}
