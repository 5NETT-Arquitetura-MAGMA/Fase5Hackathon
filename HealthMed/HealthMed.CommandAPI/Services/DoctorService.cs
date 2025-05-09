using HealthMed.CommandAPI.Interfaces.Repository;
using HealthMed.CommandAPI.Interfaces.Services;
using HealthMed.Migrator.Data.Entities;
using HealthMed.Migrator.Data.Entities.Enum;

namespace HealthMed.CommandAPI.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<List<MedicalConsultation>> GetConsultations(Guid doctorId)
        {
            try
            {
                return await _doctorRepository.GetConsultations(doctorId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<MedicalConsultation> GetConsultation(Guid id)
        {
            try
            {
                return await _doctorRepository.GetConsultation(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<MedicalConsultation> CreateConsultation(Guid userId, Guid doctorId, DateTime scheduleDate, TimeSpan scheduleTime)
        {
            try
            {
                var consultation = new MedicalConsultation()
                {
                    ScheduleTime = scheduleTime,
                    CreationTime = DateTime.Now,
                    DoctorId = doctorId,
                    PatientId = userId,
                    Id = Guid.NewGuid(),
                    ScheduledDate = scheduleDate.Date,
                    Status = ConsultationStatus.PendingConfirmation,
                };
                consultation = await _doctorRepository.CreateConsultation(consultation);
                return consultation;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<MedicalConsultation> UpdateConsultation(Guid consultationId, bool accepted, string? justification)
        {
            try
            {
                var consultation = await _doctorRepository.GetConsultation(consultationId);
                if (consultation != null)
                {
                    consultation.UpdateTime = DateTime.Now;
                    consultation.Status = accepted ? ConsultationStatus.Confirmed : ConsultationStatus.Rejected;
                    consultation.Justification = justification;
                    consultation = await _doctorRepository.UpdateConsultation(consultation);
                    return consultation;
                }
                else
                {
                    throw new Exception("Consultation not found");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<MedicalConsultation> UpdateConsultation(Guid consultationId, ConsultationStatus status, string? justification)
        {
            try
            {
                var consultation = await _doctorRepository.GetConsultation(consultationId);
                if (consultation != null)
                {
                    consultation.UpdateTime = DateTime.Now;
                    consultation.Status = status;
                    consultation.Justification = justification;
                    consultation = await _doctorRepository.UpdateConsultation(consultation);
                    return consultation;
                }
                else
                {
                    throw new Exception("Consultation not found");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task CreateOffDay(Guid doctorId, DateTime offDate)
        {
            try
            {
                var offDay = new DoctorOffDays()
                {
                    Id = Guid.NewGuid(),
                    OffDate = offDate,
                    CreationTime = DateTime.Now,
                    DoctorId = doctorId,
                };
                await _doctorRepository.CreateOffDay(offDay);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task CreateSchedule(Guid doctorId, DayOfWeek dayOfWeek, TimeSpan? startTime, TimeSpan? endTime)
        {
            try
            {
                var schedule = new DoctorSchedule()
                {
                    DoctorId = doctorId,
                    DayOfWeek = dayOfWeek,
                    StartTime = startTime,
                    EndTime = endTime,
                    CreationTime = DateTime.Now,
                    Id = Guid.NewGuid(),
                };
                await _doctorRepository.CreateSchedule(schedule);
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
    }
}