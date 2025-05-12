namespace HealthMed.Gateway.Controllers.Dto.Patient.Input
{
    public class CancelConsultationInput
    {
        public Guid ConsultationId { get; set; }
        public string? Justification { get; set; }
    }
}