namespace HealthMed.Gateway.Controllers.Dto.Doctor.Input
{
    public class CreateScheduleInput
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public Guid DoctorId { get; set; }
    }
}