using HealthMed.Migrator.Data.Entities;
using HealthMed.Migrator.Data.Entities.Enum;

namespace HealthMed.CommandAPI.Interfaces.Services
{
    public interface IDoctorService
    {
        public Task CreateOffDay(Guid doctorId, DateTime offDate);

        public Task CreateSchedule(Guid doctorId, DayOfWeek dayOfWeek, TimeSpan? startTime, TimeSpan? endTime);

        public Task<List<DoctorSchedule>> GetWorkDays(Guid id);

        public Task<List<DoctorOffDays>> GetOffDays(Guid id);

        public Task<List<MedicalConsultation>> GetConsultations(Guid doctorId);

        public Task<MedicalConsultation> CreateConsultation(Guid userId, Guid doctorId, DateTime scheduleDate, TimeSpan scheduleTime);

        public Task<MedicalConsultation> UpdateConsultation(Guid consultationId, ConsultationStatus status, string? justification);

        public Task<MedicalConsultation> UpdateConsultation(Guid consultationId, bool accepted, string? justification);

        public Task<MedicalConsultation> GetConsultation(Guid id);
    }
}