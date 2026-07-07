using Commit.Api.Models;
using Commit.Api.Models.DTOs;
using Commit.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Commit.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Commit.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;
        private readonly NameGeneratorService _nameGeneratorService;
        private readonly AppDbContext _context;
        // contructor injection for UserManager and TokenService
        public AuthController(
            UserManager<AppUser> userManager,
            TokenService tokenService, 
            NameGeneratorService nameGeneratorService,
            AppDbContext context
            )
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _nameGeneratorService = nameGeneratorService;
            _context = context;
        }

        private async Task<string> CreateAndSaveRefreshTokenAsync(AppUser user)
        {
            var refreshToken = new RefreshToken
            {
                Token = _tokenService.CreateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = false,
                AppUserId = user.Id
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                DisplayName = string.IsNullOrWhiteSpace(dto.DisplayName) 
                ? _nameGeneratorService.GenerateName() 
                : dto.DisplayName,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var token = _tokenService.CreateToken(user);
            var refreshToken = await CreateAndSaveRefreshTokenAsync(user);
            return Ok(new { token, refreshToken });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            var validPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!validPassword)
            {
                return Unauthorized("Invalid email or password");
            }

            var token = _tokenService.CreateToken(user);
            var refreshToken = await CreateAndSaveRefreshTokenAsync(user);
            return Ok(new { token, refreshToken });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh(RefreshTokenDto dto)
        {
            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == dto.RefreshToken);
            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired refresh token");
            }

            storedToken.IsRevoked = true;

            var newToken = _tokenService.CreateToken(storedToken.User);
            var newRefreshToken = await CreateAndSaveRefreshTokenAsync(storedToken.User);

            await _context.SaveChangesAsync();

            return Ok(new { token = newToken, refreshToken = newRefreshToken });
        }
    }
}
