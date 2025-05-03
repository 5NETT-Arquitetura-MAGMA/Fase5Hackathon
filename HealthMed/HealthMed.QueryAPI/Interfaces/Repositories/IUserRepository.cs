using HealthMed.Migrator.Data.Entities;

namespace HealthMed.QueryAPI.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<(List<User>, int)> GetAllDoctors(Guid? doctorId, int pageSize, int pageNumber, string? sortBy, string? sortDirection);
    }
}