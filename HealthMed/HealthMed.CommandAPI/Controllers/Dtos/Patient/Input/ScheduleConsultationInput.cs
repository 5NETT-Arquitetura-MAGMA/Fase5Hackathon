namespace HealthMed.CommandAPI.Controllers.Dtos.Patient.Input
{
    public class ScheduleConsultationInput
    {
        public Guid DoctorId { get; set; }

        public string ScheduleDate { get; set; }

        public TimeSpan ScheduleTime { get; set; }
    }
}