using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SwaggerFirstApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JwtDebugController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtDebugController> _logger;

        public JwtDebugController(IConfiguration configuration, ILogger<JwtDebugController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("config-check")]
        public IActionResult CheckConfiguration()
        {
            // Get all JWT configuration values
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            // Get entire configuration 
            var allJwtSection = _configuration.GetSection("Jwt").Get<Dictionary<string, string>>();

            // Check if Configuration has the Jwt section
            var hasJwtSection = _configuration.GetSection("Jwt").Exists();

            return Ok(new
            {
                HasJwtSection = hasJwtSection,
                DirectAccess = new
                {
                    Key = jwtKey ?? "(null)",
                    Issuer = jwtIssuer ?? "(null)",
                    Audience = jwtAudience ?? "(null)"
                },
                SectionValues = allJwtSection ?? new Dictionary<string, string>(),
                AllConfigKeys = GetAllConfigurationKeys(_configuration)
            });
        }

        // Helper method to get all configuration keys
        private List<string> GetAllConfigurationKeys(IConfiguration configuration)
        {
            var keys = new List<string>();

            void GetKeys(IConfiguration config, string parentPath = "")
            {
                foreach (var child in config.GetChildren())
                {
                    var path = string.IsNullOrEmpty(parentPath) ? child.Key : $"{parentPath}:{child.Key}";
                    keys.Add(path);

                    if (child.GetChildren().Any())
                    {
                        GetKeys(child, path);
                    }
                }
            }

            GetKeys(configuration);
            return keys;
        }

        [HttpPost("validate-token-fixed")]
        public IActionResult ValidateTokenFixed([FromBody] TokenValidationRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest(new { error = "Token no proporcionado" });
            }

            try
            {
                // Get JWT configuration with extra logging
                var jwtKey = _configuration["Jwt:Key"];
                var jwtIssuer = _configuration["Jwt:Issuer"];
                var jwtAudience = _configuration["Jwt:Audience"];

                _logger.LogInformation($"====== JWT CONFIG DEBUG ======");
                _logger.LogInformation($"Key: {jwtKey ?? "(null)"}");
                _logger.LogInformation($"Issuer: {jwtIssuer ?? "(null)"}");
                _logger.LogInformation($"Audience: {jwtAudience ?? "(null)"}");

                // IMPORTANT: Check for null values
                if (string.IsNullOrWhiteSpace(jwtIssuer))
                {
                    _logger.LogError("JWT Issuer is null or empty!");
                    // Use a fallback value if needed
                    jwtIssuer = "SwaggerFirstApp";
                }

                if (string.IsNullOrWhiteSpace(jwtAudience))
                {
                    _logger.LogError("JWT Audience is null or empty!");
                    // Use a fallback value if needed
                    jwtAudience = "SwaggerFirstAppUsers";
                }

                // Decode key
                byte[] keyBytes;
                try
                {
                    keyBytes = Convert.FromBase64String(jwtKey);
                }
                catch
                {
                    keyBytes = Encoding.UTF8.GetBytes(jwtKey);
                }

                // Configure validation
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ClockSkew = TimeSpan.Zero
                };

                // Validate token
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(request.Token, validationParameters, out var validatedToken);

                // Show token info
                var jwtToken = tokenHandler.ReadJwtToken(request.Token);

                return Ok(new
                {
                    message = "Token válido",
                    user = principal.Identity.Name,
                    claims = principal.Claims.Select(c => new { type = c.Type, value = c.Value }).ToArray(),
                    header = jwtToken.Header.ToDictionary(h => h.Key, h => h.Value),
                    payload = jwtToken.Payload.ToDictionary(p => p.Key, p => p.Value)
                });
            }
            catch (SecurityTokenExpiredException)
            {
                return BadRequest(new { error = "Token expirado" });
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                return BadRequest(new { error = "Firma del token inválida" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Token inválido", message = ex.Message });
            }
        }
    }

    public class TokenValidationRequest
    {
        public string Token { get; set; }
    }
}