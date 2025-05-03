using HealthMed.QueryAPI.Controllers.Dtos;
using Microsoft.IdentityModel.Tokens;

namespace HealthMed.QueryAPI.Utils
{
    public static class QueryParametersExtensions
    {
        public static bool HasPrevious(this QueryParams queryParameters)
        {
            return queryParameters.PageNumber > 1;
        }

        public static bool HasNext(this QueryParams queryParameters, int totalCount)
        {
            return queryParameters.PageNumber < (int)GetTotalPages(queryParameters, totalCount);
        }

        public static double GetTotalPages(this QueryParams queryParameters, int totalCount)
        {
            if (queryParameters.PageSize == 0)
            {
                queryParameters.PageSize = 1;
            }
            return Math.Ceiling(totalCount / (double)queryParameters.PageSize);
        }
    }
}