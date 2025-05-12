using HealthMed.Gateway.Controllers.Dto.User.Input;
using HealthMed.Gateway.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HealthMed.Gateway.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;

        public UserController(IConfiguration configuration, ILogger<UserController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> Create(CreateUserInput input)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var url = "";
                if (DocumentoValidator.IsCrm(input.Login))
                {
                    url = Flurl.Url.Combine(_configuration["Command"], "user", "createMedicUser");
                }
                else if (DocumentoValidator.IsCpf(input.Login))
                {
                    url = Flurl.Url.Combine(_configuration["Command"], "user", "createPatientUser");
                }
                else
                {
                    return BadRequest(new { message = "Login não está dentro do padrão necessário" });
                }

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                var content = new StringContent(JsonConvert.SerializeObject(input), null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode);
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
                return StatusCode(500, new { message = "Ocorreu um erro inesperado" });
            }
        }
    }
}