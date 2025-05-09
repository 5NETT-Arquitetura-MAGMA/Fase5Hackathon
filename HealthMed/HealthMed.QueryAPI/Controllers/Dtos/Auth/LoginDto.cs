using System.ComponentModel.DataAnnotations;

namespace HealthMed.QueryAPI.Controllers.Dtos.Auth
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}