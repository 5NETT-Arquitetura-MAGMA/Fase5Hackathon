using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthMed.CommandAPI.Interfaces.Repository;
using HealthMed.CommandAPI.Services;
using HealthMed.Migrator.Data.Entities;
using HealthMed.Migrator.Data.Entities.Enum;
using Moq;
using Xunit;

namespace HealthMed.CommandAPI.Tests.Services
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _repositoryMock;
        private readonly DoctorService _doctorService;

        public DoctorServiceTests()
        {
            _repositoryMock = new Mock<IDoctorRepository>();
            _doctorService = new DoctorService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetConsultations_ReturnsExpectedResult()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var expectedConsultations = new List<MedicalConsultation>
            {
                new MedicalConsultation { Id = Guid.NewGuid(), DoctorId = doctorId, Status = ConsultationStatus.Confirmed }
            };

            _repositoryMock
                .Setup(repo => repo.GetConsultations(doctorId))
                .ReturnsAsync(expectedConsultations);

            // Act
            var result = await _doctorService.GetConsultations(doctorId);

            // Assert
            Assert.Equal(expectedConsultations, result);
            _repositoryMock.Verify(repo => repo.GetConsultations(doctorId), Times.Once);
        }

        [Fact]
        public async Task GetConsultation_ReturnsExpectedResult()
        {
            // Arrange
            var consultationId = Guid.NewGuid();
            var expectedConsultation = new MedicalConsultation
            {
                Id = consultationId,
                Status = ConsultationStatus.PendingConfirmation
            };

            _repositoryMock
                .Setup(repo => repo.GetConsultation(consultationId))
                .ReturnsAsync(expectedConsultation);

            // Act
            var result = await _doctorService.GetConsultation(consultationId);

            // Assert
            Assert.Equal(expectedConsultation, result);
            _repositoryMock.Verify(repo => repo.GetConsultation(consultationId), Times.Once);
        }

        [Fact]
        public async Task CreateConsultation_ReturnsCreatedConsultation()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var doctorId = Guid.NewGuid();
            var scheduleDate = DateTime.Now;
            var scheduleTime = new TimeSpan(10, 0, 0);
            var expectedConsultation = new MedicalConsultation
            {
                Id = Guid.NewGuid(),
                DoctorId = doctorId,
                PatientId = userId,
                ScheduledDate = scheduleDate.Date,
                ScheduleTime = scheduleTime,
                Status = ConsultationStatus.PendingConfirmation
            };

            _repositoryMock
                .Setup(repo => repo.CreateConsultation(It.IsAny<MedicalConsultation>()))
                .ReturnsAsync(expectedConsultation);

            // Act
            var result = await _doctorService.CreateConsultation(userId, doctorId, scheduleDate, scheduleTime);

            // Assert
            Assert.Equal(expectedConsultation, result);
            _repositoryMock.Verify(repo => repo.CreateConsultation(It.IsAny<MedicalConsultation>()), Times.Once);
        }

        [Fact]
        public async Task UpdateConsultation_WithAcceptedStatus_ReturnsUpdatedConsultation()
        {
            // Arrange
            var consultationId = Guid.NewGuid();
            var justification = "Approved";
            var existingConsultation = new MedicalConsultation
            {
                Id = consultationId,
                Status = ConsultationStatus.PendingConfirmation
            };

            var updatedConsultation = new MedicalConsultation
            {
                Id = consultationId,
                Status = ConsultationStatus.Confirmed,
                Justification = justification
            };

            _repositoryMock
                .Setup(repo => repo.GetConsultation(consultationId))
                .ReturnsAsync(existingConsultation);

            _repositoryMock
                .Setup(repo => repo.UpdateConsultation(It.IsAny<MedicalConsultation>()))
                .ReturnsAsync(updatedConsultation);

            // Act
            var result = await _doctorService.UpdateConsultation(consultationId, true, justification);

            // Assert
            Assert.Equal(updatedConsultation, result);
            _repositoryMock.Verify(repo => repo.GetConsultation(consultationId), Times.Once);
            _repositoryMock.Verify(repo => repo.UpdateConsultation(It.IsAny<MedicalConsultation>()), Times.Once);
        }

        [Fact]
        public async Task CreateOffDay_CreatesOffDaySuccessfully()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var offDate = DateTime.Now;

            _repositoryMock
                .Setup(repo => repo.CreateOffDay(It.IsAny<DoctorOffDays>()))
                .ReturnsAsync(new DoctorOffDays());

            // Act
            await _doctorService.CreateOffDay(doctorId, offDate);

            // Assert
            _repositoryMock.Verify(repo => repo.CreateOffDay(It.IsAny<DoctorOffDays>()), Times.Once);
        }

        [Fact]
        public async Task CreateSchedule_CreatesScheduleSuccessfully()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var dayOfWeek = DayOfWeek.Monday;
            var startTime = new TimeSpan(9, 0, 0);
            var endTime = new TimeSpan(17, 0, 0);

            _repositoryMock
                .Setup(repo => repo.CreateSchedule(It.IsAny<DoctorSchedule>()))
                .ReturnsAsync(new DoctorSchedule());

            // Act
            await _doctorService.CreateSchedule(doctorId, dayOfWeek, startTime, endTime);

            // Assert
            _repositoryMock.Verify(repo => repo.CreateSchedule(It.IsAny<DoctorSchedule>()), Times.Once);
        }
    }
}
