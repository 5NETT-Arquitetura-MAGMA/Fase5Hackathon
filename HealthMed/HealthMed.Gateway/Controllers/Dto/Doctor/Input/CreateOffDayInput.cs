namespace HealthMed.Gateway.Controllers.Dto.Doctor.Input
{
    public class CreateOffDayInput
    {
        public DateTime OffDate { get; set; }
        public Guid DoctorId { get; set; }
    }
}