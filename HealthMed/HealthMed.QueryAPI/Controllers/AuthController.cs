using HealthMed.QueryAPI.Controllers.Dtos.Auth;
using HealthMed.QueryAPI.Interfaces.Services;
using HealthMed.QueryAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthMed.QueryAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _service;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration, IUserService service, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _service = service;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                var user = await _service.Get(model.Username);
                if (user == null || (user != null && user.Id == Guid.Empty))
                {
                    _logger.LogError("Usuário não encontrado");
                    return NotFound(new { message = "Usuário não encontrado" });
                }

                if (!VerifyPassword(model.Password, user.Password, user.SecurityHash))
                {
                    _logger.LogError("Credenciais inválidas.");
                    return Unauthorized(new { message = "Credenciais inválidas." });
                }

                var token = GenerateJwtToken(model.Username);

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Ocorreu um erro inesperado" });
            }
        }

        #region Private Methods

        private bool VerifyPassword(string enteredPassword, string storedHashedPassword, string securityHash)
        {
            string hashedPasswordToCheck = PasswordUtils.HashPassword(enteredPassword, securityHash);

            return hashedPasswordToCheck == storedHashedPassword;
        }

        private string GenerateJwtToken(string username)
        {
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtSettings:Secret").Value!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, username),
                }),
                Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JwtSettings:TokenLifetimeInMinutes")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration.GetSection("JwtSettings:Issuer").Value,
                Audience = _configuration.GetSection("JwtSettings:Audience").Value
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        #endregion Private Methods
    }
}