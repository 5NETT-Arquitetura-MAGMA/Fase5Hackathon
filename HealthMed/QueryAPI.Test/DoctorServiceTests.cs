using HealthMed.Migrator.Data.Entities;
using HealthMed.Migrator.Data.Entities.Enum;
using HealthMed.QueryAPI.Interfaces.Repositories;
using HealthMed.QueryAPI.Services;
using Moq;

namespace QueryAPI.Test
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
        public async Task GetOffDays_ReturnsExpectedResult()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var expectedOffDays = new List<DoctorOffDays>
            {
                new DoctorOffDays { Id = Guid.NewGuid(), DoctorId = doctorId, OffDate = DateTime.Now }
            };

            _repositoryMock
                .Setup(repo => repo.GetOffDays(doctorId))
                .ReturnsAsync(expectedOffDays);

            // Act
            var result = await _doctorService.GetOffDays(doctorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOffDays, result);
            _repositoryMock.Verify(repo => repo.GetOffDays(doctorId), Times.Once);
        }

        [Fact]
        public async Task GetWorkDays_ReturnsExpectedResult()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var expectedWorkDays = new List<DoctorSchedule>
            {
                new DoctorSchedule { Id = Guid.NewGuid(), DoctorId = doctorId, DayOfWeek = DayOfWeek.Monday }
            };

            _repositoryMock
                .Setup(repo => repo.GetWorkDays(doctorId))
                .ReturnsAsync(expectedWorkDays);

            // Act
            var result = await _doctorService.GetWorkDays(doctorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedWorkDays, result);
            _repositoryMock.Verify(repo => repo.GetWorkDays(doctorId), Times.Once);
        }

        [Fact]
        public async Task GetAllDoctors_ReturnsExpectedResult()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var expectedDoctors = new List<User>
            {
                new User { Id = doctorId, Name = "Dr. John Doe" }
            };

            _repositoryMock
                .Setup(repo => repo.GetAllDoctors(doctorId, 10, 1, null, null))
                .ReturnsAsync((expectedDoctors, expectedDoctors.Count));

            // Act
            var (result, total) = await _doctorService.GetAllDoctors(doctorId, 10, 1, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDoctors, result);
            Assert.Equal(expectedDoctors.Count, total);
            _repositoryMock.Verify(repo => repo.GetAllDoctors(doctorId, 10, 1, null, null), Times.Once);
        }

        [Fact]
        public async Task ListMedicalConsultation_ReturnsExpectedResult()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var expectedConsultations = new List<MedicalConsultation>
            {
                new MedicalConsultation { Id = Guid.NewGuid(), DoctorId = doctorId, Status = ConsultationStatus.Confirmed }
            };

            _repositoryMock
                .Setup(repo => repo.ListMedicalConsultation(doctorId, 10, 1, null, null))
                .ReturnsAsync((expectedConsultations, expectedConsultations.Count));

            // Act
            var (result, total) = await _doctorService.ListMedicalConsultation(doctorId, 10, 1, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedConsultations, result);
            Assert.Equal(expectedConsultations.Count, total);
            _repositoryMock.Verify(repo => repo.ListMedicalConsultation(doctorId, 10, 1, null, null), Times.Once);
        }

        [Fact]
        public async Task ListPendingMedicalConsultation_ReturnsExpectedResult()
        {
            // Arrange
            var doctorId = Guid.NewGuid();
            var expectedConsultations = new List<MedicalConsultation>
            {
                new MedicalConsultation { Id = Guid.NewGuid(), DoctorId = doctorId, Status = ConsultationStatus.PendingConfirmation }
            };

            _repositoryMock
                .Setup(repo => repo.ListPendingMedicalConsultation(doctorId, 10, 1, null, null))
                .ReturnsAsync((expectedConsultations, expectedConsultations.Count));

            // Act
            var (result, total) = await _doctorService.ListPendingMedicalConsultation(doctorId, 10, 1, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedConsultations, result);
            Assert.Equal(expectedConsultations.Count, total);
            _repositoryMock.Verify(repo => repo.ListPendingMedicalConsultation(doctorId, 10, 1, null, null), Times.Once);
        }

        [Fact]
        public async Task ListPatientMedicalConsultation_ReturnsExpectedResult()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var expectedConsultations = new List<MedicalConsultation>
            {
                new MedicalConsultation { Id = Guid.NewGuid(), PatientId = patientId, Status = ConsultationStatus.Confirmed }
            };

            _repositoryMock
                .Setup(repo => repo.ListPatientMedicalConsultation(patientId, 10, 1, null, null))
                .ReturnsAsync((expectedConsultations, expectedConsultations.Count));

            // Act
            var (result, total) = await _doctorService.ListPatientMedicalConsultation(patientId, 10, 1, null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedConsultations, result);
            Assert.Equal(expectedConsultations.Count, total);
            _repositoryMock.Verify(repo => repo.ListPatientMedicalConsultation(patientId, 10, 1, null, null), Times.Once);
        }
    }
}
