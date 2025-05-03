namespace HealthMed.QueryAPI.Controllers.Dtos.Doctor.Output
{
    public class PaginationOutput<T>
    {
        public List<T> Value { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public double TotalPages { get; set; }
    }
}