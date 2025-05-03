using HealthMed.Migrator.Data.Entities;

namespace HealthMed.QueryAPI.Interfaces.Services
{
    public interface IUserService
    {
        public Task<(List<User>, int)> GetAllDoctors(Guid? doctorId, int pageSize, int pageNumber, string? sortBy, string? sortDirection);
    }
}