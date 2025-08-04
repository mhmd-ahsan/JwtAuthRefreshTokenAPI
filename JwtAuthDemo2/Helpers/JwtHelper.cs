using JwtAuthDemo2.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JwtAuthDemo2.Helpers
{
    public class JwtHelper
    {
        //    private readonly IConfiguration _config;

        //    public JwtHelper(IConfiguration config)
        //    {
        //        _config = config;   
        //    }

        //    public string GenerateAccessToken(User user)
        //    {
        //        var claims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Name, user.Username),
        //            new Claim(ClaimTypes.Role, user.Role),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //        };

        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        //        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


        //        var token = new JwtSecurityToken(
        //            issuer: _config["Jwt:Issuer"],
        //            audience: _config["Jwt:Audience"],
        //            expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:DurationInMinutes"]!)),
        //            claims: claims,
        //            signingCredentials: cred
        //            );


        //        return new JwtSecurityTokenHandler().WriteToken(token);
        //    }

        //    public string GenerateRefreshToken()
        //    {
        //        var randomBytes = new byte[64];
        //        using var rng = RandomNumberGenerator.Create();
        //        rng.GetBytes(randomBytes);
        //        return Convert.ToBase64String(randomBytes);
        //    }

        //    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        //    {
        //        if(token == null) return null;

        //        var tokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidateAudience = true,
        //            ValidateIssuer = true,
        //            ValidateIssuerSigningKey = true,
        //            ValidateLifetime = false,
        //            ValidIssuer = _config["Jwt:Issuer"],
        //            ValidAudience = _config["Jwt:Audience"],
        //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)),
        //        };


        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        return tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
        //    }


        private readonly IConfiguration _config;

        public JwtHelper(IConfiguration config)
        {
            _config = config;
        }


        public string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:DurationInMinutes"]!)),
                claims: claims,
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }


        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            if (token == null) return null;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!))

            }; 

            var  tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
        }


    }
}
