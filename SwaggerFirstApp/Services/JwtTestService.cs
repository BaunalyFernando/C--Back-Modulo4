using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SwaggerFirstApp.Services
{
    public class JwtTestService
    {
        private readonly byte[] _originalKeyBytes;
        private readonly byte[] _testKeyBytes;

        public JwtTestService(byte[] originalKeyBytes, byte[] testKeyBytes)
        {
            _originalKeyBytes = originalKeyBytes;
            _testKeyBytes = testKeyBytes;
        }

        public string GenerateTokenWithOriginalKey(string username)
        {
            return GenerateToken(username, _originalKeyBytes);
        }

        public string GenerateTokenWithTestKey(string username)
        {
            return GenerateToken(username, _testKeyBytes);
        }

        private string GenerateToken(string username, byte[] keyBytes)
        {
            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateTokenWithOriginalKey(string token)
        {
            return ValidateToken(token, _originalKeyBytes);
        }

        public bool ValidateTokenWithTestKey(string token)
        {
            return ValidateToken(token, _testKeyBytes);
        }

        private bool ValidateToken(string token, byte[] keyBytes)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(keyBytes);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

