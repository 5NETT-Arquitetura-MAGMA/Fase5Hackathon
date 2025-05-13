namespace HealthMed.Gateway.Controllers.Dto
{
    public class PaginationOutput<T>
    {
        public List<T> Value { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public double TotalPages { get; set; }
    }

    public class SchedulePaginationOutput<T>
    {
        public List<T> Value { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
    }
}