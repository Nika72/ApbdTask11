using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using task11.Interfaces;
using task11.Models;

namespace task11.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            var user = new User
            {
                Username = registerUserDto.Username,
                PasswordHash = registerUserDto.Password
            };

            await _userService.RegisterAsync(user);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            var user = await _userService.LoginAsync(loginUserDto.Username, loginUserDto.Password);
            var token = await _userService.GenerateJwtTokenAsync(user);
            var refreshToken = await _userService.GenerateRefreshTokenAsync(user);

            return Ok(new
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var user = await _userService.GetUserByRefreshTokenAsync(refreshTokenDto.RefreshToken);
            if (user == null) return Unauthorized();

            var token = await _userService.GenerateJwtTokenAsync(user);
            var refreshToken = await _userService.GenerateRefreshTokenAsync(user);

            return Ok(new
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }
    }

    public class RegisterUserDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoginUserDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
