using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HealthMed.CommandAPI.Controllers;
using HealthMed.CommandAPI.Controllers.Dtos.Doctor.Input;
using HealthMed.CommandAPI.Controllers.Dtos.Patient.Input;
using HealthMed.CommandAPI.Interfaces.Services;
using HealthMed.Migrator.Data.Entities;
using HealthMed.Migrator.Data.Entities.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HealthMed.CommandAPI.Tests.Integration.Controllers
{
    public class PatientControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IDoctorService> _doctorServiceMock;
        private readonly Mock<ILogger<PatientController>> _loggerMock;
        private readonly PatientController _controller;

        public PatientControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _doctorServiceMock = new Mock<IDoctorService>();
            _loggerMock = new Mock<ILogger<PatientController>>();

            _controller = new PatientController(
                _userServiceMock.Object,
                _doctorServiceMock.Object,
                _loggerMock.Object
            );

            // Simula o contexto HTTP
            _controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Adiciona um token JWT válido ao cabeçalho de autorização
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer valid_token";
        }

        [Fact]
        public async Task CancelConsultation_ReturnsOk_WhenValidRequest()
        {
            // Arrange
            var input = new CancelConsultationInput
            {
                ConsultationId = Guid.NewGuid(),
                Justification = "Patient cannot attend"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Type = UserType.Patient
            };

            var consultation = new MedicalConsultation
            {
                Id = input.ConsultationId,
                PatientId = user.Id
            };

            _userServiceMock
                .Setup(service => service.Get(It.IsAny<string>()))
                .ReturnsAsync(user);

            _doctorServiceMock
                .Setup(service => service.GetConsultation(input.ConsultationId))
                .ReturnsAsync(consultation);

            // Update the following line in the `CancelConsultation_ReturnsOk_WhenValidRequest` test method:

            _doctorServiceMock
                .Setup(service => service.UpdateConsultation(input.ConsultationId, ConsultationStatus.Canceled, input.Justification))
                .ReturnsAsync(consultation); // Change from `Task.CompletedTask` to `ReturnsAsync(consultation)` to match the expected return type.


            // Act
            var result = await _controller.CancelConsultation(input);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode);
        }

        [Fact]
        public async Task ScheduleConsultation_ReturnsCreated_WhenValidRequest()
        {
            // Arrange
            var input = new ScheduleConsultationInput
            {
                DoctorId = Guid.NewGuid(),
                ScheduleDate = "15/05/2025",
                ScheduleTime = new TimeSpan(10, 0, 0)
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Type = UserType.Patient
            };

            var doctorWorkDays = new List<DoctorSchedule>
            {
                new DoctorSchedule { DayOfWeek = DayOfWeek.Thursday }
            };

            var doctorOffDays = new List<DoctorOffDays>();

            var consultations = new List<MedicalConsultation>();

            var createdConsultation = new MedicalConsultation
            {
                Id = Guid.NewGuid(),
                DoctorId = input.DoctorId,
                PatientId = user.Id,
                ScheduledDate = DateTime.Parse("15/05/2025"),
                ScheduleTime = input.ScheduleTime,
                Status = ConsultationStatus.PendingConfirmation
            };

            _userServiceMock
                .Setup(service => service.Get(It.IsAny<string>()))
                .ReturnsAsync(user);

            _doctorServiceMock
                .Setup(service => service.GetWorkDays(input.DoctorId))
                .ReturnsAsync(doctorWorkDays);

            _doctorServiceMock
                .Setup(service => service.GetOffDays(input.DoctorId))
                .ReturnsAsync(doctorOffDays);

            _doctorServiceMock
                .Setup(service => service.GetConsultations(input.DoctorId))
                .ReturnsAsync(consultations);

            _doctorServiceMock
                .Setup(service => service.CreateConsultation(user.Id, input.DoctorId, It.IsAny<DateTime>(), input.ScheduleTime))
                .ReturnsAsync(createdConsultation);

            // Act
            var result = await _controller.ScheduleConsultation(input);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<Microsoft.AspNetCore.Mvc.ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.Created, actionResult.StatusCode);

            var responseValue = JsonSerializer.Deserialize<dynamic>(JsonSerializer.Serialize(actionResult.Value));
            Assert.Equal(createdConsultation.Id.ToString(), responseValue?.Id.ToString());
        }
    }
}
