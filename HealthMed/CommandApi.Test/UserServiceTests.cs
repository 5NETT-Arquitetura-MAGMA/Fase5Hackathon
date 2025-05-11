using System;
using System.Threading.Tasks;
using HealthMed.CommandAPI.Interfaces.Repository;
using HealthMed.CommandAPI.Services;
using HealthMed.Migrator.Data.Entities;
using HealthMed.Migrator.Data.Entities.Enum;
using Moq;
using Xunit;

namespace HealthMed.CommandAPI.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetUserByCPF_ReturnsExpectedUser()
        {
            // Arrange
            var cpf = "12345678900";
            var expectedUser = new User
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Login = cpf,
                Type = UserType.Patient
            };

            _repositoryMock
                .Setup(repo => repo.GetUserByCPF(cpf))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetUserByCPF(cpf);

            // Assert
            Assert.Equal(expectedUser, result);
            _repositoryMock.Verify(repo => repo.GetUserByCPF(cpf), Times.Once);
        }

        [Fact]
        public async Task GetDoctorByCRM_ReturnsExpectedUser()
        {
            // Arrange
            var crm = "CRM12345";
            var expectedUser = new User
            {
                Id = Guid.NewGuid(),
                Name = "Dr. Test",
                Login = crm,
                Type = UserType.Doctor
            };

            _repositoryMock
                .Setup(repo => repo.GetDoctorByCRM(crm))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetDoctorByCRM(crm);

            // Assert
            Assert.Equal(expectedUser, result);
            _repositoryMock.Verify(repo => repo.GetDoctorByCRM(crm), Times.Once);
        }

        [Fact]
        public async Task Get_ById_ReturnsExpectedUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expectedUser = new User
            {
                Id = userId,
                Name = "Test User",
                Login = "testuser",
                Type = UserType.Patient
            };

            _repositoryMock
                .Setup(repo => repo.Get(userId))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.Get(userId);

            // Assert
            Assert.Equal(expectedUser, result);
            _repositoryMock.Verify(repo => repo.Get(userId), Times.Once);
        }

        [Fact]
        public async Task Get_ByLogin_ReturnsExpectedUser()
        {
            // Arrange
            var login = "testuser";
            var expectedUser = new User
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Login = login,
                Type = UserType.Patient
            };

            _repositoryMock
                .Setup(repo => repo.Get(login))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.Get(login);

            // Assert
            Assert.Equal(expectedUser, result);
            _repositoryMock.Verify(repo => repo.Get(login), Times.Once);
        }

        [Fact]
        public async Task CreateUser_CreatesUserSuccessfully()
        {
            // Arrange  
            var name = "Test User";
            var phoneNumber = "123456789";
            var emailAddress = "testuser@example.com";
            var login = "43143932899"; // CPF  
            var password = "password123";
            var specialty = "Cardiology";

            var user = new User
            {
                Name = name,
                PhoneNumber = phoneNumber,
                EmailAddress = emailAddress,
                Login = login,
                Password = password,
                Specialty = specialty,
                Type = UserType.Doctor // Assuming the user is a doctor for this test  
            };

            _repositoryMock
                .Setup(repo => repo.CreateUser(It.IsAny<User>()))
                .ReturnsAsync(user);

            // Act  
            await _userService.CreateUser(name, phoneNumber, emailAddress, login, password, specialty);

            // Assert  
            _repositoryMock.Verify(repo => repo.CreateUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task GetUserByCPF_ThrowsException_WhenUserNotFound()
        {
            // Arrange
            var cpf = "00000000000";

            _repositoryMock
                .Setup(repo => repo.GetUserByCPF(cpf))
                .ThrowsAsync(new Exception("User not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userService.GetUserByCPF(cpf));
            _repositoryMock.Verify(repo => repo.GetUserByCPF(cpf), Times.Once);
        }

        [Fact]
        public async Task GetDoctorByCRM_ThrowsException_WhenUserNotFound()
        {
            // Arrange
            var crm = "INVALID_CRM";

            _repositoryMock
                .Setup(repo => repo.GetDoctorByCRM(crm))
                .ThrowsAsync(new Exception("User not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userService.GetDoctorByCRM(crm));
            _repositoryMock.Verify(repo => repo.GetDoctorByCRM(crm), Times.Once);
        }
    }
}
