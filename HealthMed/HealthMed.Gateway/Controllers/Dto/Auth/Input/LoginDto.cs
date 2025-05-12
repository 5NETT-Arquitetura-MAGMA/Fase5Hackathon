using System.ComponentModel.DataAnnotations;

namespace HealthMed.Gateway.Controllers.Dto.Auth.Input
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}