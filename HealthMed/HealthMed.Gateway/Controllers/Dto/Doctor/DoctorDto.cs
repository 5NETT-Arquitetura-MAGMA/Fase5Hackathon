using System.ComponentModel.DataAnnotations;

namespace HealthMed.Gateway.Controllers.Dto.Doctor
{
    public class DoctorDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        public string? Specialty { get; set; }
    }
}