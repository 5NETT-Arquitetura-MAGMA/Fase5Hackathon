using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HealthMed.CommandAPI.Controllers;
using HealthMed.CommandAPI.Controllers.Dtos.Doctor.Input;
using HealthMed.CommandAPI.Interfaces.Services;
using HealthMed.Migrator.Data.Entities;
using HealthMed.Migrator.Data.Entities.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HealthMed.CommandAPI.Tests.Integration.Controllers
{
    public class DoctorControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IDoctorService> _doctorServiceMock;
        private readonly Mock<ILogger<DoctorController>> _loggerMock;
        private readonly DoctorController _controller;

        public DoctorControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _doctorServiceMock = new Mock<IDoctorService>();
            _loggerMock = new Mock<ILogger<DoctorController>>();

            _controller = new DoctorController(
                _userServiceMock.Object,
                _doctorServiceMock.Object,
                _loggerMock.Object
            );

            // Simula o contexto HTTP
            _controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
            {
                HttpContext = new DefaultHttpContext()

            };
        }

        [Fact]
        public async Task UpdateConsultation_ReturnsOk_WhenValidRequest()
        {
            // Arrange
            var input = new UpdateConsultationInput
            {
                ConsultationId = Guid.NewGuid(),
                Accepted = true,
                Justification = "Approved"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Type = UserType.Doctor
            };

            var consultation = new MedicalConsultation
            {
                Id = input.ConsultationId,
                DoctorId = user.Id
            };

            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer valid_token";

            _userServiceMock
                .Setup(service => service.Get(It.IsAny<string>()))
                .ReturnsAsync(user);

            _doctorServiceMock
                .Setup(service => service.GetConsultation(input.ConsultationId))
                .ReturnsAsync(consultation);

            _doctorServiceMock
                .Setup(service => service.UpdateConsultation(input.ConsultationId, input.Accepted, input.Justification))
                .ReturnsAsync(consultation); // Fix: Ensure the return type matches Task<MedicalConsultation>

            // Act
            var result = await _controller.UpdateConsultation(input);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode);
        }

        [Fact]
        public async Task CreateOffDay_ReturnsCreated_WhenValidRequest()
        {
            // Arrange
            var input = new CreateOffDayInput
            {
                DoctorId = Guid.NewGuid(),
                OffDate = DateTime.Now
            };

            var user = new User
            {
                Id = input.DoctorId,
                Type = UserType.Doctor
            };

            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer valid_token";

            _userServiceMock
                .Setup(service => service.Get(It.IsAny<string>()))
                .ReturnsAsync(user);

            _doctorServiceMock
                .Setup(service => service.CreateOffDay(input.DoctorId, input.OffDate))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateOffDay(input);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<Microsoft.AspNetCore.Mvc.CreatedResult>(result);
            Assert.Equal((int)HttpStatusCode.Created, actionResult.StatusCode);
        }




        [Fact]
        public async Task CreateSchedule_ReturnsCreated_WhenValidRequest()
        {
            // Arrange
            var input = new CreateScheduleInput
            {
                DoctorId = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Monday,
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(17, 0, 0)
            };

            var user = new User
            {
                Id = input.DoctorId,
                Type = UserType.Doctor
            };

            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer valid_token";

            _userServiceMock
                .Setup(service => service.Get(It.IsAny<string>()))
                .ReturnsAsync(user);

            _doctorServiceMock
                .Setup(service => service.CreateSchedule(input.DoctorId, input.DayOfWeek, input.StartTime, input.EndTime))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateSchedule(input);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<Microsoft.AspNetCore.Mvc.CreatedResult>(result);
            Assert.Equal((int)HttpStatusCode.Created, actionResult.StatusCode);
        }

        [Fact]
        public async Task UpdateConsultation_ReturnsUnauthorized_WhenUserIsNotDoctor()
        {
            // Arrange
            var input = new UpdateConsultationInput
            {
                ConsultationId = Guid.NewGuid(),
                Accepted = true,
                Justification = "Approved"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Type = UserType.Patient // Not a doctor
            };

            

            _userServiceMock
                .Setup(service => service.Get(It.IsAny<string>()))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.UpdateConsultation(input);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<Microsoft.AspNetCore.Mvc.StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.Unauthorized, actionResult.StatusCode);
        }
    }
}
