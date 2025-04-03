using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SwaggerFirstApp.Data;
using SwaggerFirstApp.Services.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SwaggerFirstApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CredentialsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CredentialsController> _logger;

        public CredentialsController(AppDbContext context, IConfiguration configuration, ILogger<CredentialsController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login(CredentialsDto credentials)
        {
            _logger.LogInformation($"Intento de login para: {credentials.Username}");

            var user = _context.Credentials.FirstOrDefault(u => u.Username == credentials.Username);
            if (user == null || user.Password != credentials.Password)
            {
                _logger.LogWarning($"Credenciales inválidas para: {credentials.Username}");
                return Unauthorized();
            }

            var token = GenerateToken(credentials.Username);
            _logger.LogInformation($"Login exitoso para: {credentials.Username}");

            return Ok(new { token });
        }

        [HttpGet("verify-token")]
        [Authorize]
        public IActionResult VerifyToken()
        {
            var username = User.Identity?.Name;

            _logger.LogInformation($"Identity.Name: {username ?? "null"}");
            _logger.LogInformation($"Identity.IsAuthenticated: {User.Identity?.IsAuthenticated}");

            var allClaims = User.Claims.Select(c => new { type = c.Type, value = c.Value }).ToList();
            _logger.LogInformation($"Claims encontrados: {allClaims.Count}");

            foreach (var claim in allClaims)
            {
                _logger.LogInformation($"Claim: {claim.type} = {claim.value}");
            }

            return Ok(new
            {
                message = "Token validado correctamente",
                user = username,
                isAuthenticated = User.Identity?.IsAuthenticated ?? false,
                claims = allClaims
            });
        }

        [HttpGet("test-token")]
        public IActionResult TestToken()
        {
            var username = "test-user";
            var key = _configuration["Jwt:Key"];

            byte[] keyBytes;
            try
            {
                keyBytes = Convert.FromBase64String(key);
            }
            catch (Exception)
            {
                keyBytes = Encoding.UTF8.GetBytes(key);
            }

            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                message = "Usa este token para probar la autenticación",
                configuration = new
                {
                    issuer = _configuration["Jwt:Issuer"],
                    audience = _configuration["Jwt:Audience"],
                    keyExists = !string.IsNullOrEmpty(key)
                }
            });
        }

        private string GenerateToken(string username)
        {
            var key = _configuration["Jwt:Key"];
            _logger.LogInformation($"Generando token para: {username}");

            if (string.IsNullOrEmpty(key) || key.Length < 32)
            {
                throw new InvalidOperationException("La clave JWT debe tener al menos 32 caracteres.");
            }

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, username)
    };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            _logger.LogInformation("Token generado correctamente");

            return tokenString;
        }

    }
}