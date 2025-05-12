using Newtonsoft.Json;

namespace HealthMed.Gateway.Controllers.Dto.Auth.Output
{
    public class LoginOutput
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }
    }
}