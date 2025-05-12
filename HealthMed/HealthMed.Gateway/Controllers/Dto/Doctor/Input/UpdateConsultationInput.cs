namespace HealthMed.Gateway.Controllers.Dto.Doctor.Input
{
    public class UpdateConsultationInput
    {
        public Guid ConsultationId { get; set; }
        public bool Accepted { get; set; }
        public string? Justification { get; set; }
    }
}