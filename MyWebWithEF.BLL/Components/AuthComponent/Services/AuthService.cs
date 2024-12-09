using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Jose;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyWebWithEF.BLL.Enums;
using MyWebWithEF.BLL.Settings;
using JwtSettings = MyWebWithEF.BLL.Settings.JwtSettings;

namespace MyWebWithEF.BLL.Services
{
    public interface IAuthService
    {
        string Authenticate(string username, string password, out UserRole role);
    }

    public class AuthService : IAuthService
    {
        private readonly JwtSettings _jwtSettings;

        public AuthService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string Authenticate(string username, string password, out UserRole role)
        {
            role = UserRole.User;

            // Validate credentials
            if (username == "test" && password == "test")
            {
                role = UserRole.User;
                return GenerateJwtToken(username, role);
            }

            if (username == "admin" && password == "admin")
            {
                role = UserRole.Admin;
                return GenerateJwtToken(username, role);
            }

            return null; // Authentication failed
        }

        private string GenerateJwtToken(string username, UserRole role)
        {
            // Generate a key from the settings
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            // Make sure the key is of sufficient size
            if (key.KeySize < 128)
            {
                throw new ArgumentOutOfRangeException(nameof(_jwtSettings.Key), "Key must be at least 128 bits.");
            }

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}