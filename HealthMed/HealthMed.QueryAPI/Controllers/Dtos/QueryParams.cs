﻿namespace HealthMed.QueryAPI.Controllers.Dtos
{
    public class QueryParams
    {
        private int _pageSize = 10;
        private const int MaxPageSize = 50;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } = "asc";
    }
}