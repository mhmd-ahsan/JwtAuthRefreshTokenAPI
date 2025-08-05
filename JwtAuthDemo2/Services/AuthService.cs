using JwtAuthDemo2.Data;
using JwtAuthDemo2.Helpers;
using JwtAuthDemo2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthDemo2.Services
{
    public class AuthService : IAuthService
    {
        
        private readonly ApplicationDbContext _context;
        private readonly JwtHelper _jwtHelper;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _hasher;


        public AuthService(ApplicationDbContext context, JwtHelper jwtHelper, IConfiguration configuration, PasswordHasher<User> hasher)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _configuration = configuration;
            _hasher = hasher;
        }

        public async Task<string> RegisterAsync(string username, string password, string role = "User")
        {
            if (await _context.Users.AnyAsync(u => u.Username == username))
                throw new Exception("User alredy exists");

            var user = new User
            {
                Username = username,
                Role = role,
                PasswordHash = _hasher.HashPassword(null!, password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "Registeration successfull.";
        }

        public async Task<(string accessToken, string refreshToken)> LoginAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new Exception("Invalid credentials");
            }

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid credntials.");

            var accessToken = _jwtHelper.GenerateAccessToken(user);
            var refreshToken = _jwtHelper.GenerateRefreshToken();


            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"]!));

            return (accessToken, refreshToken);
        }

        public async Task<(string newAccessToken, string newRefreshToken)> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            var principal = _jwtHelper.GetPrincipalFromExpiredToken(accessToken);
            var username = principal?.Identity?.Name;


            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                throw new Exception("Invalid refresh token");
            }


            var newAccessToken = _jwtHelper.GenerateAccessToken(user);
            var newRefreshToken = _jwtHelper.GenerateRefreshToken();


            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(int.Parse((_configuration["Jwt:RefreshTokenExpirationDays"]!)));

            await _context.SaveChangesAsync();

            return (newAccessToken, newRefreshToken);
        }
    }
}
