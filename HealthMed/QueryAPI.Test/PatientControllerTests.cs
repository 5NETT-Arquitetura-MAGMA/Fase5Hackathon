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
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjQ5Ny4xMzMuODAwLTQ1IiwibmJmIjoxNzQ3MDE0NzQ0LCJleHAiOjE3NDcwMTgzNDQsImlhdCI6MTc0NzAxNDc0NCwiaXNzIjoiaGVhbHRobWVkLmtyZWF0aS5jb20uYnIiLCJhdWQiOiJoZWFsdGhtZWQua3JlYXRpLmNvbS5iciJ9.nLxvd3vZ9wl5RzyrEf0sezUm7jjTeDuuZ3Cd5VTaiHM";
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
            var actionResult = Assert.IsType<Microsoft.AspNetCore.Mvc.StatusCodeResult>(result.Result); // Verifica se é StatusCodeResult
            Assert.Equal((int)HttpStatusCode.Unauthorized, actionResult.StatusCode);
        }


    }
}
