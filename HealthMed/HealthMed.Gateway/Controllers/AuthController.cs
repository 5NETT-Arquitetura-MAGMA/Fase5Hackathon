using HealthMed.Gateway.Controllers.Dto.Auth.Input;
using HealthMed.Gateway.Controllers.Dto.Auth.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HealthMed.Gateway.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginOutput>> Login([FromBody] LoginDto model)
        {
            try
            {
                var url = Flurl.Url.Combine(_configuration["Query"], "auth", "login");

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                var content = new StringContent(JsonConvert.SerializeObject(model), null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var output = JsonConvert.DeserializeObject<LoginOutput>(await response.Content.ReadAsStringAsync());
                    return StatusCode((int)response.StatusCode, output);
                }
                else
                {
                    var resp = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(resp))
                    {
                        return StatusCode((int)response.StatusCode, JsonConvert.DeserializeObject<Dictionary<string, object>>(resp));
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                //return StatusCode(500, new { message = "Ocorreu um erro inesperado" });
                throw;
            }
        }
    }
}