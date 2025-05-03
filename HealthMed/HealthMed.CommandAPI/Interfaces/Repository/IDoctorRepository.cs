using HealthMed.Migrator.Data.Entities;

namespace HealthMed.CommandAPI.Interfaces.Repository
{
    public interface IDoctorRepository
    {
        public Task<DoctorSchedule> CreateSchedule(DoctorSchedule schedule);
    }
}