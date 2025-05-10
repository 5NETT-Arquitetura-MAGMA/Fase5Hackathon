using HealthMed.Migrator.Data.Entities;
using HealthMed.QueryAPI.Interfaces.Repositories;
using HealthMed.QueryAPI.Interfaces.Services;

namespace HealthMed.QueryAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<User> Get(string login)
        {
            try
            {
                return await _repository.Get(login);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<User> Get(Guid id)
        {
            try
            {
                return await _repository.Get(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}