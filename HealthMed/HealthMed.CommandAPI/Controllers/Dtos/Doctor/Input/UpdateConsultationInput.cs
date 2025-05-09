namespace HealthMed.CommandAPI.Controllers.Dtos.Doctor.Input
{
    public class UpdateConsultationInput
    {
        public Guid ConsultationId { get; set; }
        public string? Justification { get; set; }
    }
}