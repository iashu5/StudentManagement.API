using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ActionResult<ApiResponse<object>> Login([FromBody] LoginDto login)
        {
            var token = _authService.Login(login);
            if (string.IsNullOrEmpty(token))
                return Unauthorized(ApiResponse<object>.Fail("Invalid credentials"));

            return Ok(ApiResponse<object>.Ok(new { Token = token }, "Login successful"));
        }
    }
}