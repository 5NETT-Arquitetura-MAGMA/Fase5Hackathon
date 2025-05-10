namespace HealthMed.QueryAPI.Controllers.Dtos.Doctor.Input
{
    public class ListDoctorQueryParams : QueryParams
    {
        public Guid? DoctorId { get; set; }
    }
}