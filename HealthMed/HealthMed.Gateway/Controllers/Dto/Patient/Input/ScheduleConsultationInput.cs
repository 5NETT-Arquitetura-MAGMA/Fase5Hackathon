namespace HealthMed.Gateway.Controllers.Dto.Patient.Input
{
    public class ScheduleConsultationInput
    {
        public Guid DoctorId { get; set; }

        public string ScheduleDate { get; set; }

        public TimeSpan ScheduleTime { get; set; }
    }
}