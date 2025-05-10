using HealthMed.Migrator.Data.Entities;

namespace HealthMed.QueryAPI.Interfaces.Services
{
    public interface IUserService
    {
        public Task<User> Get(Guid id);

        public Task<User> Get(string login);
    }
}