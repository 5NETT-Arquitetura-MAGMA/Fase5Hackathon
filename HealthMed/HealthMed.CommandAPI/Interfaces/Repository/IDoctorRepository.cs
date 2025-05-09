using HealthMed.Migrator.Data.Entities;

namespace HealthMed.CommandAPI.Interfaces.Repository
{
    public interface IDoctorRepository
    {
        public Task<DoctorOffDays> CreateOffDay(DoctorOffDays offDay);

        public Task<DoctorSchedule> CreateSchedule(DoctorSchedule schedule);

        public Task<List<DoctorSchedule>> GetWorkDays(Guid id);

        public Task<List<DoctorOffDays>> GetOffDays(Guid id);

        public Task<List<MedicalConsultation>> GetConsultations(Guid doctorId);

        public Task<MedicalConsultation> GetConsultation(Guid id);

        public Task<MedicalConsultation> CreateConsultation(MedicalConsultation consultation);

        public Task<MedicalConsultation> UpdateConsultation(MedicalConsultation consultation);
    }
}