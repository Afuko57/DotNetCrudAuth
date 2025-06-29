using Microsoft.AspNetCore.Mvc;
using MyApiTest.Services;

namespace MyApiTest.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;

        public AuthController(AuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Credential req)
        {
            var success = await _service.Register(req.Username, req.Password);
            return success ? Ok("registered") : BadRequest("username taken");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Credential req)
        {
            var success = await _service.Login(req.Username, req.Password);
            return success ? Ok("login success") : Unauthorized("invalid");
        }
    }

    public record Credential(string Username, string Password);
}
