using Flurl;
using HealthMed.Gateway.Controllers.Dto.Doctor.Input;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace HealthMed.Gateway.Controllers
{
    [ApiController]
    [Route("doctor")]
    public class DoctorController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DoctorController> _logger;

        public DoctorController(ILogger<DoctorController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("updateConsultation")]
        public async Task<ActionResult> UpdateConsultation(UpdateConsultationInput input)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    return StatusCode(401);
                }

                var url = Flurl.Url.Combine(_configuration["Command"], "doctor", "updateConsultation");

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                var content = new StringContent(JsonConvert.SerializeObject(input), null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Ocorreu um erro inesperado" });
            }
        }

        [HttpPost]
        [Route("createOffDay")]
        public async Task<ActionResult> CreateOffDay(CreateOffDayInput input)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    return StatusCode(401);
                }
                var url = Flurl.Url.Combine(_configuration["Command"], "doctor", "createOffDay");
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                var content = new StringContent(JsonConvert.SerializeObject(input), null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Ocorreu um erro inesperado" });
            }
        }

        [HttpPost]
        [Route("createSchedule")]
        public async Task<ActionResult> CreateSchedule(CreateScheduleInput input)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    return StatusCode(401);
                }

                var url = Flurl.Url.Combine(_configuration["Command"], "doctor", "createSchedule");
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                var content = new StringContent(JsonConvert.SerializeObject(input), null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Ocorreu um erro inesperado" });
            }
        }

        [HttpGet]
        [Route("consultations")]
        public async Task<ActionResult> GetConsultations([FromQuery] GetDoctorConsultationsParams param)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    return StatusCode(401);
                }

                var url = Flurl.Url.Combine(_configuration["Query"], "doctor", "consultations");
                var queryParams = new Dictionary<string, object>();

                if (param.PageNumber > 0)
                    queryParams["PageNumber"] = param.PageNumber;
                if (param.PageSize > -1)
                    queryParams["PageSize"] = param.PageSize;

                if (!string.IsNullOrEmpty(param.SortBy))
                    queryParams["SortBy"] = param.SortBy;

                if (!string.IsNullOrEmpty(param.SortDirection))
                    queryParams["SortDirection"] = param.SortDirection;

                var fullUrl = url.SetQueryParams(queryParams);

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
                request.Headers.Add("Authorization", $"Bearer {token}");
                var response = await client.SendAsync(request);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Ocorreu um erro inesperado" });
            }
        }

        [HttpGet]
        [Route("pendingConsultations")]
        public async Task<ActionResult> GetPendingConsultations([FromQuery] GetPendingConsultationsParams param)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    return StatusCode(401);
                }
                var url = Flurl.Url.Combine(_configuration["Query"], "doctor", "pendingConsultations");
                var queryParams = new Dictionary<string, object>();

                if (param.PageNumber > 0)
                    queryParams["PageNumber"] = param.PageNumber;
                if (param.PageSize > -1)
                    queryParams["PageSize"] = param.PageSize;

                if (!string.IsNullOrEmpty(param.SortBy))
                    queryParams["SortBy"] = param.SortBy;

                if (!string.IsNullOrEmpty(param.SortDirection))
                    queryParams["SortDirection"] = param.SortDirection;

                var fullUrl = url.SetQueryParams(queryParams);

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
                request.Headers.Add("Authorization", $"Bearer {token}");
                var response = await client.SendAsync(request);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Ocorreu um erro inesperado" });
            }
        }

        [HttpGet]
        [Route("{doctorId}")]
        public async Task<ActionResult> GetDoctorSchedule([FromQuery] GetDoctorScheduleParams param, Guid doctorId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    return StatusCode(401);
                }
                var url = Flurl.Url.Combine(_configuration["Query"], "doctor", doctorId.ToString());
                var queryParams = new Dictionary<string, object>();

                if (param.PageNumber > 0)
                    queryParams["PageNumber"] = param.PageNumber;
                if (param.PageSize > -1)
                    queryParams["PageSize"] = param.PageSize;

                if (!string.IsNullOrEmpty(param.SortBy))
                    queryParams["SortBy"] = param.SortBy;

                if (!string.IsNullOrEmpty(param.SortDirection))
                    queryParams["SortDirection"] = param.SortDirection;

                var fullUrl = url.SetQueryParams(queryParams);

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
                request.Headers.Add("Authorization", $"Bearer {token}");
                var response = await client.SendAsync(request);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Ocorreu um erro inesperado" });
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> List([FromQuery] ListDoctorQueryParams param)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    return StatusCode(401);
                }
                var url = Flurl.Url.Combine(_configuration["Query"], "doctor");
                var queryParams = new Dictionary<string, object>();

                if (param.PageNumber > 0)
                    queryParams["PageNumber"] = param.PageNumber;
                if (param.PageSize > -1)
                    queryParams["PageSize"] = param.PageSize;

                if (!string.IsNullOrEmpty(param.SortBy))
                    queryParams["SortBy"] = param.SortBy;

                if (!string.IsNullOrEmpty(param.SortDirection))
                    queryParams["SortDirection"] = param.SortDirection;

                var fullUrl = url.SetQueryParams(queryParams);

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
                request.Headers.Add("Authorization", $"Bearer {token}");
                var response = await client.SendAsync(request);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Ocorreu um erro inesperado" });
            }
        }
    }
}