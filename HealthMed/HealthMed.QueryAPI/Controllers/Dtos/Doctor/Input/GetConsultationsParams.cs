namespace HealthMed.QueryAPI.Controllers.Dtos.Doctor.Input
{
    public class GetConsultationsParams : QueryParams
    {
        public new string? SortDirection { get; set; } = "desc";
    }
}