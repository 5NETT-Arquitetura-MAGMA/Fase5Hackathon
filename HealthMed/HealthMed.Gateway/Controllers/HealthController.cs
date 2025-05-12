using Microsoft.AspNetCore.Mvc;

namespace HealthMed.Gateway.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public HealthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ready")]
        public async Task<ActionResult> Ready()
        {
            try
            {
                var url = Flurl.Url.Combine(_configuration["Query"], "health", "live");
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            try
            {
                var url = Flurl.Url.Combine(_configuration["Command"], "health", "live");
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok();
        }

        [HttpGet]
        [Route("live")]
        public async Task<ActionResult> Live()
        {
            try
            {
                var url = Flurl.Url.Combine(_configuration["Query"], "health", "live");
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            try
            {
                var url = Flurl.Url.Combine(_configuration["Command"], "health", "live");
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok();
        }
    }
}