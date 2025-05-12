using System;
using System.Net;
using System.Threading.Tasks;
using HealthMed.CommandAPI.Controllers;
using HealthMed.CommandAPI.Controllers.Dtos.User.Input;
using HealthMed.CommandAPI.Interfaces.Services;
using HealthMed.Migrator.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HealthMed.CommandAPI.Tests.Integration.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<UserController>> _loggerMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<UserController>>();

            _controller = new UserController(
                _userServiceMock.Object,
                _loggerMock.Object
            );

            // Simula o contexto HTTP
            _controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Fact]
        public async Task CreatePatientUser_ReturnsCreated_WhenValidRequest()
        {
            // Arrange
            var input = new CreatePatientUserInput
            {
                Name = "John Doe",
                PhoneNumber = "123456789",
                EmailAddress = "johndoe@example.com",
                Login = "123.456.789-00", // CPF válido
                Password = "Password@123"
            };

            _userServiceMock
                .Setup(service => service.GetUserByCPF(input.Login))
                .ReturnsAsync((User)null); // Simula que o usuário não existe

            _userServiceMock
                .Setup(service => service.CreateUser(input.Name, input.PhoneNumber, input.EmailAddress, input.Login, input.Password, null))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreatePatientUser(input);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<Microsoft.AspNetCore.Mvc.CreatedResult>(result);
            Assert.Equal((int)HttpStatusCode.Created, actionResult.StatusCode);
        }

        [Fact]
        public async Task CreatePatientUser_ReturnsBadRequest_WhenUserAlreadyExists()
        {
            // Arrange
            var input = new CreatePatientUserInput
            {
                Name = "John Doe",
                PhoneNumber = "123456789",
                EmailAddress = "johndoe@example.com",
                Login = "123.456.789-00", // CPF válido
                Password = "Password@123"
            };

            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Name = "Existing User",
                Login = input.Login
            };

            _userServiceMock
                .Setup(service => service.GetUserByCPF(input.Login))
                .ReturnsAsync(existingUser); // Simula que o usuário já existe

            // Act
            var result = await _controller.CreatePatientUser(input);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, actionResult.StatusCode);
            Assert.Equal("Paciente ja existe em nossa base de dados", actionResult.Value);
        }

        [Fact]
        public async Task CreateMedicUser_ReturnsCreated_WhenValidRequest()
        {
            // Arrange
            var input = new CreateMedicUserInput
            {
                Name = "Dr. Jane Doe",
                PhoneNumber = "987654321",
                EmailAddress = "janedoe@example.com",
                Login = "CRM-SP 12345", // CRM válido
                Password = "Password@123",
                Specialty = "Cardiology"
            };

            _userServiceMock
                .Setup(service => service.GetDoctorByCRM(input.Login))
                .ReturnsAsync((User)null); // Simula que o médico não existe

            _userServiceMock
                .Setup(service => service.CreateUser(input.Name, input.PhoneNumber, input.EmailAddress, input.Login, input.Password, input.Specialty))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateMedicUser(input);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<Microsoft.AspNetCore.Mvc.CreatedResult>(result);
            Assert.Equal((int)HttpStatusCode.Created, actionResult.StatusCode);
        }

        [Fact]
        public async Task CreateMedicUser_ReturnsBadRequest_WhenDoctorAlreadyExists()
        {
            // Arrange
            var input = new CreateMedicUserInput
            {
                Name = "Dr. Jane Doe",
                PhoneNumber = "987654321",
                EmailAddress = "janedoe@example.com",
                Login = "CRM-SP 12345", // CRM válido
                Password = "Password@123",
                Specialty = "Cardiology"
            };

            var existingDoctor = new User
            {
                Id = Guid.NewGuid(),
                Name = "Existing Doctor",
                Login = input.Login
            };

            _userServiceMock
                .Setup(service => service.GetDoctorByCRM(input.Login))
                .ReturnsAsync(existingDoctor); // Simula que o médico já existe

            // Act
            var result = await _controller.CreateMedicUser(input);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, actionResult.StatusCode);
            Assert.Equal("Medico ja existe em nossa base de dados", actionResult.Value);
        }
    }
}
