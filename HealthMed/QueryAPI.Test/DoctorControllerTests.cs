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

            // Adiciona um token JWT válido ao cabeçalho de autorização
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer valid_token";
        }

        [Fact]
        public async Task GetConsultations_ReturnsOk_WhenValidRequest()
        {
            // Arrange
            var param = new GetDoctorConsultationsParams
            {
                PageSize = 10,
                PageNumber = 1,
                SortBy = "ScheduledDate",
                SortDirection = "asc"
            };

            var doctor = new User
            {
                Id = Guid.NewGuid(),
                Type = UserType.Doctor
            };

            var consultations = new List<MedicalConsultation>
            {
                new MedicalConsultation
                {
                    Id = Guid.NewGuid(),
                    PatientId = Guid.NewGuid(),
                    ScheduledDate = DateTime.Now,
                    ScheduleTime = TimeSpan.FromHours(10),
                    Status = ConsultationStatus.Confirmed
                }
            };

            var patient = new User
            {
                Id = consultations[0].PatientId,
                Name = "John Doe"
            };

            _userServiceMock
                .Setup(service => service.Get(It.IsAny<string>()))
                .ReturnsAsync(doctor);

            _doctorServiceMock
                .Setup(service => service.ListMedicalConsultation(doctor.Id, param.PageSize, param.PageNumber, param.SortBy, param.SortDirection))
                .ReturnsAsync((consultations, consultations.Count));

            _userServiceMock
                .Setup(service => service.Get(consultations[0].PatientId))
                .ReturnsAsync(patient);

            // Act
            var result = await _controller.GetConsultations(param);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode);

            var responseValue = JsonSerializer.Deserialize<PaginationOutput<ConsultationDto>>(JsonSerializer.Serialize(actionResult.Value));
            Assert.NotNull(responseValue);
            Assert.Single(responseValue.Value);
            Assert.Equal(patient.Name, responseValue.Value[0].PatientName);
        }

        [Fact]
        public async Task GetPendingConsultations_ReturnsOk_WhenValidRequest()
        {
            // Arrange
            var param = new GetPendingConsultationsParams
            {
                PageSize = 10,
                PageNumber = 1,
                SortBy = "ScheduledDate",
                SortDirection = "asc"
            };

            var doctor = new User
            {
                Id = Guid.NewGuid(),
                Type = UserType.Doctor
            };

            var consultations = new List<MedicalConsultation>
            {
                new MedicalConsultation
                {
                    Id = Guid.NewGuid(),
                    PatientId = Guid.NewGuid(),
                    ScheduledDate = DateTime.Now,
                    ScheduleTime = TimeSpan.FromHours(10),
                    Status = ConsultationStatus.PendingConfirmation
                }
            };

            var patient = new User
            {
                Id = consultations[0].PatientId,
                Name = "John Doe"
            };

            _userServiceMock
                .Setup(service => service.Get(It.IsAny<string>()))
                .ReturnsAsync(doctor);

            _doctorServiceMock
                .Setup(service => service.ListPendingMedicalConsultation(doctor.Id, param.PageSize, param.PageNumber, param.SortBy, param.SortDirection))
                .ReturnsAsync((consultations, consultations.Count));

            _userServiceMock
                .Setup(service => service.Get(consultations[0].PatientId))
                .ReturnsAsync(patient);

            // Act
            var result = await _controller.GetPendingConsultations(param);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode);

            var responseValue = JsonSerializer.Deserialize<PaginationOutput<ConsultationDto>>(JsonSerializer.Serialize(actionResult.Value));
            Assert.NotNull(responseValue);
            Assert.Single(responseValue.Value);
            Assert.Equal(patient.Name, responseValue.Value[0].PatientName);
        }

        [Fact]
        public async Task List_ReturnsOk_WhenValidRequest()
        {
            // Arrange
            var param = new ListDoctorQueryParams
            {
                PageSize = 10,
                PageNumber = 1,
                SortBy = "Name",
                SortDirection = "asc"
            };

            var doctors = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Dr. John Doe",
                    EmailAddress = "johndoe@example.com",
                    PhoneNumber = "123456789",
                    Specialty = "Cardiology"
                }
            };

            _userServiceMock
                .Setup(service => service.Get(It.IsAny<string>()))
                .ReturnsAsync(new User { Type = UserType.Patient });

            _doctorServiceMock
                .Setup(service => service.GetAllDoctors(param.DoctorId, param.PageSize, param.PageNumber, param.SortBy, param.SortDirection))
                .ReturnsAsync((doctors, doctors.Count));

            // Act
            var result = await _controller.List(param);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode);

            var responseValue = JsonSerializer.Deserialize<PaginationOutput<DoctorDto>>(JsonSerializer.Serialize(actionResult.Value));
            Assert.NotNull(responseValue);
            Assert.Single(responseValue.Value);
            Assert.Equal(doctors[0].Name, responseValue.Value[0].Name);
        }
    }
}
