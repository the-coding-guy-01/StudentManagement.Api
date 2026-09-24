using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentManagement.Api.Data;
using StudentManagement.Api.DTOs.Auth;
using StudentManagement.Api.Models;
using StudentManagement.Api.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagement.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<AppUser> _passwordHasher;


        public AuthService(
            ApplicationDbContext context,
            IConfiguration configuration,
            IPasswordHasher<AppUser> passwordHasher)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }


        public async Task<LoginResponseDto?> LoginAsync(
            LoginRequestDto request)
        {
            var user = await _context.AppUsers
                .FirstOrDefaultAsync(x =>
                    x.Username == request.Username);


            if (user == null)
            {
                return null;
            }


            if (!user.IsActive)
            {
                return null;
            }


            var passwordResult =
                _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    request.Password);


            if (passwordResult ==
                PasswordVerificationResult.Failed)
            {
                return null;
            }


            var expiresAt =
                DateTime.UtcNow.AddHours(
                    _configuration.GetValue<int>(
                        "Jwt:ExpiryHours"));


            var token = GenerateToken(
                user,
                expiresAt);


            return new LoginResponseDto
            {
                UserId = user.Id,
                Username = user.Username,
                Role = user.Role,
                Token = token,
                ExpiresAt = expiresAt
            };
        }


        private string GenerateToken(
            AppUser user,
            DateTime expiresAt)
        {
            var key =
                _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException(
                    "JWT key is missing.");


            var issuer =
                _configuration["Jwt:Issuer"];


            var audience =
                _configuration["Jwt:Audience"];


            var securityKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key));


            var credentials =
                new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256);


            var claims = new List<Claim>
            {
                new(
                    JwtRegisteredClaimNames.Sub,
                    user.Id.ToString()),

                new(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString()),

                new(
                    ClaimTypes.Name,
                    user.Username),

                new(
                    ClaimTypes.Role,
                    user.Role),

                new(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
            };


            if (user.StudentId.HasValue)
            {
                claims.Add(
                    new Claim(
                        "StudentId",
                        user.StudentId.Value.ToString()));
            }


            if (user.TeacherId.HasValue)
            {
                claims.Add(
                    new Claim(
                        "TeacherId",
                        user.TeacherId.Value.ToString()));
            }


            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}