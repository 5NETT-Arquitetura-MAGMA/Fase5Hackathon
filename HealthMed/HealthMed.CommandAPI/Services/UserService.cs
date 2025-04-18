using HealthMed.CommandAPI.Interfaces.Services;
using HealthMed.CommandAPI.Utils;
using HealthMed.Migrator.Data.Entities;
using HealthMed.Migrator.Data.Entities.Enum;

namespace HealthMed.CommandAPI.Services
{
    public class UserService : IUserService
    {
        public UserService()
        { }

        public async Task CreateUser(string name, string phoneNumber, string emailAddress, string login, string password)
        {
            try
            {
                var type = ValidateUserType(login);
                var hash = PasswordUtils.GenerateSecurityHash();
                var newPassword = PasswordUtils.HashPassword(password, hash);
                var user = new User()
                {
                    Name = name,
                    PhoneNumber = phoneNumber,
                    EmailAddress = emailAddress,
                    Login = login,
                    Password = newPassword,
                    CreationTime = DateTime.Now,
                    DoctorConsultationStatus = null,
                    DoctorSchedules = null,
                    Id = Guid.NewGuid(),
                    PatientConsultationStatus = null,
                    SecurityHash = hash,
                    Specialty = null,
                    Type = type,
                    UpdateTime = DateTime.Now
                };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private UserType ValidateUserType(string login)
        {
            var validate = DocumentoValidator.DeterminarTipoDocumento(login);
            if (validate == DocumentoTipo.Cpf)
            {
                return UserType.Patient;
            }
            else if (validate == DocumentoTipo.Crm)
            {
                return UserType.Doctor;
            }
            else
            {
                throw new Exception("Login informado invalido");
            }
        }
    }
}