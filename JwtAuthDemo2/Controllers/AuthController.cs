using JwtAuthDemo2.Dtos;
using JwtAuthDemo2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthDemo2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
                var result = await _auth.RegisterAsync(dto.Username, dto.Password, dto.Role);
                return Ok(new { message = result });
            }

            catch (Exception ex)
            {
                return BadRequest(new {error = ex.Message});
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var (accessToken, refreshToken) = await _auth.LoginAsync(dto.Username, dto.Password);
                return Ok(new { accessToken, refreshToken });
            }

            catch (Exception ex)
            {
                return Unauthorized(new {error = ex.Message});
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenDto dto)
        {
            try
            {
                var (accessToken, refreshToken) = await _auth.RefreshTokenAsync(dto.AccessToken, dto.RefreshToken);
                return Ok(new { accessToken, refreshToken });
            }

            catch (Exception ex)
            {
                return Unauthorized(new {error = ex.Message});
            }
        }
    }
}
