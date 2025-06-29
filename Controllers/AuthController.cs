using Microsoft.AspNetCore.Mvc;
using MyApiTest.Services;
using MyApiTest.DTOs;

namespace MyApiTest.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] AuthRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .SelectMany(x => x.Value!.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToList();

                    var validationResponse = new AuthResponseDto(
                        success: false,
                        message: "Invalid data",
                        code: "VALIDATION_ERROR"
                    );

                    return BadRequest(validationResponse);
                }

                var result = await _authService.RegisterAsync(request);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new AuthResponseDto(
                    success: false,
                    message: "Internal server error",
                    code: "INTERNAL_ERROR"
                );

                Console.WriteLine($"Register Error: {ex.Message}");

                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] AuthRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .SelectMany(x => x.Value!.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToList();

                    var validationResponse = new AuthResponseDto(
                        success: false,
                        message: "Invalid data",
                        code: "VALIDATION_ERROR"
                    );

                    return BadRequest(validationResponse);
                }

                var result = await _authService.LoginAsync(request);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return Unauthorized(result);
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new AuthResponseDto(
                    success: false,
                    message: "Internal server error",
                    code: "INTERNAL_ERROR"
                );

                Console.WriteLine($"Login Error: {ex.Message}");

                return StatusCode(500, errorResponse);
            }
        }

        [HttpGet("health")]
        public ActionResult<AuthResponseDto> Health()
        {
            var response = new AuthResponseDto(true, "System is running normally", "SUCCESS");
            return Ok(response);
        }
    }
}
