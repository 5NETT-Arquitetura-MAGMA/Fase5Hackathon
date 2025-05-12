namespace HealthMed.Gateway.Controllers.Dto.Patient.Input
{
    public class GetConsultationsParams : QueryParams
    {
        public new string? SortDirection { get; set; } = "desc";
    }
}