namespace HealthMed.CommandAPI.Interfaces.Services
{
    public interface IDoctorService
    {
        public Task CreateSchedule(Guid doctorId, DayOfWeek dayOfWeek, TimeSpan? startTime, TimeSpan? endTime);
    }
}