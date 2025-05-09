using HealthMed.Migrator.Data.Entities;

namespace HealthMed.QueryAPI.Interfaces.Services
{
    public interface IUserService
    {
        public Task<(List<User>, int)> GetAllDoctors(Guid? doctorId, int pageSize, int pageNumber, string? sortBy, string? sortDirection);

        public Task<User> Get(Guid id);

        public Task<User> Get(string login);

        public Task<List<DoctorSchedule>> GetWorkDays(Guid id);

        public Task<List<DoctorOffDays>> GetOffDays(Guid id);
    }
}