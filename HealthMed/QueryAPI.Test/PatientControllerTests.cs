using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using HealthMed.Migrator.Data.Entities;
using HealthMed.Migrator.Data.Entities.Enum;
using HealthMed.QueryAPI.Controllers;
using HealthMed.QueryAPI.Controllers.Dtos.Doctor;
using HealthMed.QueryAPI.Controllers.Dtos.Doctor.Input;
using HealthMed.QueryAPI.Controllers.Dtos.Doctor.Output;
using HealthMed.QueryAPI.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace QueryAPI.Test
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
        public async Task GetConsultations_ReturnsOk_WhenValidRequest()
        {
            // Arrange
            var param = new GetConsultationsParams
            {
                PageSize = 10,
                PageNumber = 1,
                SortBy = "ScheduledDate",
                SortDirection = "asc"
            };

            var patient = new User
            {
                Id = Guid.NewGuid(),
                Type = UserType.Patient
            };

            var consultations = new List<MedicalConsultation>
            {
                new MedicalConsultation
                {
                    Id = Guid.NewGuid(),
                    PatientId = patient.Id,
                    ScheduledDate = DateTime.Now,
                    ScheduleTime = TimeSpan.FromHours(10),
                    Status = ConsultationStatus.Confirmed
                }
            };

            _userServiceMock
                .Setup(service => service.Get(It.IsAny<string>()))
                .ReturnsAsync(patient);

            _doctorServiceMock
                .Setup(service => service.ListPatientMedicalConsultation(patient.Id, param.PageSize, param.PageNumber, param.SortBy, param.SortDirection))
                .ReturnsAsync((consultations, consultations.Count));

            // Act
            var result = await _controller.GetConsultations(param);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode);

            var responseValue = JsonSerializer.Deserialize<PaginationOutput<ConsultationDto>>(JsonSerializer.Serialize(actionResult.Value));
            Assert.NotNull(responseValue);
            Assert.Single(responseValue.Value);
            Assert.Equal(consultations[0].Id, responseValue.Value[0].Id);
        }

        [Fact]
        public async Task GetConsultations_ReturnsUnauthorized_WhenUserIsNotPatient()
        {
            // Arrange
            var param = new GetConsultationsParams
            {
                PageSize = 10,
                PageNumber = 1,
                SortBy = "ScheduledDate",
                SortDirection = "asc"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Type = UserType.Doctor // Not a patient
            };

            _userServiceMock
                .Setup(service => service.Get(It.IsAny<string>()))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.GetConsultations(param);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<Microsoft.AspNetCore.Mvc.StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.Unauthorized, actionResult.StatusCode);
        }

        [Fact]
        public async Task GetConsultations_ReturnsNotFound_WhenPatientDoesNotExist()
        {
            // Arrange
            var param = new GetConsultationsParams
            {
                PageSize = 10,
                PageNumber = 1,
                SortBy = "ScheduledDate",
                SortDirection = "asc"
            };

            _userServiceMock
                .Setup(service => service.Get(It.IsAny<string>()))
                .ReturnsAsync((User)null); // Simula que o paciente não existe

            // Act
            var result = await _controller.GetConsultations(param);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<Microsoft.AspNetCore.Mvc.StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, actionResult.StatusCode);
        }
    }
}
