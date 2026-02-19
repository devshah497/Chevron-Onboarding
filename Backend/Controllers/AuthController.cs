using Backend.Data;
using Backend.Models.DTOs.Auth;
using Backend.Models.Entities;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _db;
        private readonly JwtTokenService _jwt;
        private readonly IConfiguration _config;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext db,
            JwtTokenService jwt,
            IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _jwt = jwt;
            _config = config;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(RegisterRequest request)
        {
            await EnsureRoles();

            var existing = await _userManager.FindByEmailAsync(request.Email);
            if (existing != null) return BadRequest("User already exists");

            var user = new ApplicationUser { UserName = request.Email, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Admin");

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwt.CreateAccessToken(user, roles);

            var refreshToken = await CreateRefreshToken(user.Id);

            return Ok(new { accessToken, refreshToken, role = roles.FirstOrDefault() });
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return Unauthorized("Invalid credentials");

            var valid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!valid) return Unauthorized("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwt.CreateAccessToken(user, roles);

            var refreshToken = await CreateRefreshToken(user.Id);

            return Ok(new { accessToken, refreshToken, role = roles.FirstOrDefault() });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return Ok(new { message = "If user exists, reset token generated." });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // For now we return token directly (since no email service configured yet)
            return Ok(new { resetToken = token });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return BadRequest("Invalid request");

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { message = "Password reset successful" });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshRequest request)
        {
            var stored = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == request.RefreshToken);
            if (stored == null || stored.IsRevoked || stored.ExpiresAtUtc < DateTime.UtcNow)
                return Unauthorized("Invalid refresh token");

            var user = await _userManager.FindByIdAsync(stored.UserId);
            if (user == null) return Unauthorized("Invalid refresh token");

            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _jwt.CreateAccessToken(user, roles);

            return Ok(new { accessToken = newAccessToken });
        }

        // Logout (revoke refresh token)
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshRequest request)
        {
            var stored = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == request.RefreshToken);
            if (stored != null)
            {
                stored.IsRevoked = true;
                await _db.SaveChangesAsync();
            }
            return Ok(new { message = "Logged out" });
        }

        private async Task EnsureRoles()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await _roleManager.RoleExistsAsync("Employee"))
                await _roleManager.CreateAsync(new IdentityRole("Employee"));
        }

        private async Task<string> CreateRefreshToken(string userId)
        {
            // revoke old tokens for that user (simple approach)
            var old = await _db.RefreshTokens.Where(x => x.UserId == userId && !x.IsRevoked).ToListAsync();
            foreach (var t in old) t.IsRevoked = true;

            var token = Guid.NewGuid().ToString("N");
            var days = int.Parse(_config["Jwt:RefreshExpiryDays"] ?? "7");

            _db.RefreshTokens.Add(new RefreshToken
            {
                UserId = userId,
                Token = token,
                ExpiresAtUtc = DateTime.UtcNow.AddDays(days),
                IsRevoked = false
            });

            await _db.SaveChangesAsync();
            return token;
        }
    }
}