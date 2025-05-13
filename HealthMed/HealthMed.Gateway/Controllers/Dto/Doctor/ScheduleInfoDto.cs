namespace HealthMed.Gateway.Controllers.Dto.Doctor
{
    public class ScheduleInfoDto
    {
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string ScheduleDate { get; set; }
        public List<TimeSpan> ScheduleTimes { get; set; }
    }
}