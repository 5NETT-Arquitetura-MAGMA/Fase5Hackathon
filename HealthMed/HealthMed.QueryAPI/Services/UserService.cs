using HealthMed.Migrator.Data.Entities;
using HealthMed.QueryAPI.Interfaces.Repositories;
using HealthMed.QueryAPI.Interfaces.Services;

namespace HealthMed.QueryAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _doctorRepository;

        public UserService(IUserRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<(List<User>, int)> GetAllDoctors(Guid? doctorId, int pageSize, int pageNumber, string? sortBy, string? sortDirection)
        {
            try
            {
                var (users, total) = await _doctorRepository.GetAllDoctors(doctorId, pageSize, pageNumber, sortBy, sortDirection);
                return (users, total);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<DoctorSchedule>> GetWorkDays(Guid id)
        {
            try
            {
                return await _doctorRepository.GetWorkDays(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<DoctorOffDays>> GetOffDays(Guid id)
        {
            try
            {
                return await _doctorRepository.GetOffDays(id);
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
                return await _doctorRepository.Get(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}