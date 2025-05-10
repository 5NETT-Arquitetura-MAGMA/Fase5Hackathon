using HealthMed.Migrator.Data.Entities;

namespace HealthMed.QueryAPI.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<User> Get(Guid id);

        public Task<User> Get(string login);
    }
}