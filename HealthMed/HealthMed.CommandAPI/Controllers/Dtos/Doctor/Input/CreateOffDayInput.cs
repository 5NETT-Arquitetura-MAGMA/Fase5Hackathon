namespace HealthMed.CommandAPI.Controllers.Dtos.Doctor.Input
{
    public class CreateOffDayInput
    {
        public DateTime OffDate { get; set; }
        public Guid DoctorId { get; set; }
    }
}