using System.ComponentModel.DataAnnotations;

namespace HealthMed.CommandAPI.Controllers.Dtos.User.Input
{
    public class CreatePatientUserInput : CreateUserInput
    {
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$",
                   ErrorMessage = "O valor informado de login não é um CPF valido")]
        public new string Login { get; set; }
    }
}