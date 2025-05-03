using System.ComponentModel.DataAnnotations;

namespace HealthMed.CommandAPI.Controllers.Dtos.User.Input
{
    public class CreateMedicUserInput : CreateUserInput
    {
        [RegularExpression(@"^CRM-[A-Z]{2} \d+$|^CRM\/[A-Z]{2} \d+$",
                   ErrorMessage = "O valor informado de login não é um CRM valido")]
        public new string Login { get; set; }

        [Required]
        public string Specialty { get; set; }
    }
}