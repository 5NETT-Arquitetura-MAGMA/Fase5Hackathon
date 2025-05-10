using HealthMed.Migrator.Data.Entities;
using HealthMed.QueryAPI.Interfaces.Repositories;
using HealthMed.QueryAPI.Interfaces.Services;

namespace HealthMed.QueryAPI.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;

        public DoctorService(IDoctorRepository repository)
        {
            _repository = repository;
        }

        public async Task<(List<DoctorOffDays>, int)> GetOffDays(Guid id, int pageSize, int pageNumber, string? sortBy, string? sortDirection)
        {
            try
            {
                var (res, total) = await _repository.GetOffDays(id, pageSize, pageNumber, sortBy, sortDirection);
                return (res, total);
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
                return await _repository.GetOffDays(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<(List<DoctorSchedule>, int)> GetWorkDays(Guid id, int pageSize, int pageNumber, string? sortBy, string? sortDirection)
        {
            try
            {
                var (res, total) = await _repository.GetWorkDays(id, pageSize, pageNumber, sortBy, sortDirection);
                return (res, total);
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
                return await _repository.GetWorkDays(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<(List<User>, int)> GetAllDoctors(Guid? doctorId, int pageSize, int pageNumber, string? sortBy, string? sortDirection)
        {
            try
            {
                var (res, total) = await _repository.GetAllDoctors(doctorId, pageSize, pageNumber, sortBy, sortDirection);
                return (res, total);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<(List<MedicalConsultation>, int)> ListMedicalConsultation(Guid doctorId, int pageSize, int pageNumber, string? sortBy, string? sortDirection)
        {
            try
            {
                var (res, total) = await _repository.ListMedicalConsultation(doctorId, pageSize, pageNumber, sortBy, sortDirection);
                return (res, total);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<(List<MedicalConsultation>, int)> ListPendingMedicalConsultation(Guid doctorId, int pageSize, int pageNumber, string? sortBy, string? sortDirection)
        {
            try
            {
                var (res, total) = await _repository.ListPendingMedicalConsultation(doctorId, pageSize, pageNumber, sortBy, sortDirection);
                return (res, total);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<(List<MedicalConsultation>, int)> ListPatientMedicalConsultation(Guid patientId, int pageSize, int pageNumber, string? sortBy, string? sortDirection)
        {
            try
            {
                var (res, total) = await _repository.ListPatientMedicalConsultation(patientId, pageSize, pageNumber, sortBy, sortDirection);
                return (res, total);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}