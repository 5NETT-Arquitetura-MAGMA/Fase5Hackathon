using System.ComponentModel.DataAnnotations;

namespace HealthMed.Gateway.Controllers.Dto.User.Input
{
    public class CreateUserInput
    {
        [Required(ErrorMessage = "O Nome é obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O Telefone é obrigatório.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "O Email é obrigatório.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "O Login é obrigatório.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "A Senha é obrigatória.")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d]).{12,}$",
                          ErrorMessage = "A Senha deve ter no mínimo 12 caracteres, uma maiúscula, uma minúscula, um número e um caractere especial.")]
        public string Password { get; set; }

        public string? Specialty { get; set; }
    }
}