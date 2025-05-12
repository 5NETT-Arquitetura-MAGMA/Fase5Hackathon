namespace HealthMed.Gateway.Controllers.Dto.Doctor.Input
{
    public class ListDoctorQueryParams : QueryParams
    {
        public Guid? DoctorId { get; set; }
    }
}