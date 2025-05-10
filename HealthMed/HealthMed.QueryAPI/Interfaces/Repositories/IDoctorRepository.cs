using HealthMed.Migrator.Data.Entities;

namespace HealthMed.QueryAPI.Interfaces.Repositories
{
    public interface IDoctorRepository
    {
        public Task<(List<MedicalConsultation>, int)> ListMedicalConsultation(Guid doctorId, int pageSize, int pageNumber, string? sortBy, string? sortDirection);

        public Task<(List<MedicalConsultation>, int)> ListPendingMedicalConsultation(Guid doctorId, int pageSize, int pageNumber, string? sortBy, string? sortDirection);

        public Task<(List<MedicalConsultation>, int)> ListPatientMedicalConsultation(Guid patientId, int pageSize, int pageNumber, string? sortBy, string? sortDirection);

        public Task<(List<User>, int)> GetAllDoctors(Guid? doctorId, int pageSize, int pageNumber, string? sortBy, string? sortDirection);

        public Task<(List<DoctorSchedule>, int)> GetWorkDays(Guid id, int pageSize, int pageNumber, string? sortBy, string? sortDirection);

        public Task<(List<DoctorOffDays>, int)> GetOffDays(Guid id, int pageSize, int pageNumber, string? sortBy, string? sortDirection);

        public Task<List<DoctorSchedule>> GetWorkDays(Guid id);

        public Task<List<DoctorOffDays>> GetOffDays(Guid id);
    }
}