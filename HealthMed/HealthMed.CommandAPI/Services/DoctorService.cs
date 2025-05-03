using HealthMed.CommandAPI.Interfaces.Repository;
using HealthMed.CommandAPI.Interfaces.Services;
using HealthMed.Migrator.Data.Entities;

namespace HealthMed.CommandAPI.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task CreateOffDay(Guid doctorId, DateTime offDate)
        {
            try
            {
                var offDay = new DoctorOffDays()
                {
                    Id = Guid.NewGuid(),
                    OffDate = offDate,
                    CreationTime = DateTime.Now,
                    DoctorId = doctorId,
                };
                await _doctorRepository.CreateOffDay(offDay);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task CreateSchedule(Guid doctorId, DayOfWeek dayOfWeek, TimeSpan? startTime, TimeSpan? endTime)
        {
            try
            {
                var schedule = new DoctorSchedule()
                {
                    DoctorId = doctorId,
                    DayOfWeek = dayOfWeek,
                    StartTime = startTime,
                    EndTime = endTime,
                    CreationTime = DateTime.Now,
                    Id = Guid.NewGuid(),
                };
                await _doctorRepository.CreateSchedule(schedule);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}