namespace HealthMed.QueryAPI.Controllers.Dtos.Doctor.Input
{
    public class DoctorQueryParams : QueryParams
    {
        public Guid? DoctorId { get; set; }
    }
}