namespace HealthMed.Gateway.Controllers.Dto.Doctor
{
    public class ConsultationDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }

        public DateTime CreationTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime ScheduledDate { get; set; }
        public TimeSpan ScheduleTime { get; set; }
        public ConsultationStatus Status { get; set; }
        public string StatusStr { get; set; }
        public string? Justification { get; set; }
    }

    public enum ConsultationStatus
    {
        PendingConfirmation, Confirmed, Rejected, Canceled
    }
}