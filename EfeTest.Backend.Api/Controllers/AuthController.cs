using EfeTest.Backend.Application.DTOs.Request;
using EfeTest.Backend.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EfeTest.Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService _authService)
        {
            this._authService = _authService;   
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var res = await _authService.Login(request);
            return Ok(new { msg = "", data = res });
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            var res = await _authService.Register(request);
            return Ok(new { msg = "", data = res });
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            var res = await _authService.VerifyAndGenerateToken(request);
            return Ok(new { msg = "", data = res });
        }
    }
}
