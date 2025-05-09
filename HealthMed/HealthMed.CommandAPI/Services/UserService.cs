using HealthMed.CommandAPI.Interfaces.Repository;
using HealthMed.CommandAPI.Interfaces.Services;
using HealthMed.CommandAPI.Utils;
using HealthMed.Migrator.Data.Entities;
using HealthMed.Migrator.Data.Entities.Enum;

namespace HealthMed.CommandAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByCPF(string cpf)
        {
            try
            {
                return await _userRepository.GetUserByCPF(cpf);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<User> GetDoctorByCRM(string crm)
        {
            try
            {
                return await _userRepository.GetDoctorByCRM(crm);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<User> Get(Guid id)
        {
            try
            {
                return await _userRepository.Get(id);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<User> Get(string login)
        {
            try
            {
                return await _userRepository.Get(login);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task CreateUser(string name, string phoneNumber, string emailAddress, string login, string password, string? specialty)
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
                    Specialty = specialty,
                    Type = type,
                    UpdateTime = DateTime.Now
                };
                await _userRepository.CreateUser(user);
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