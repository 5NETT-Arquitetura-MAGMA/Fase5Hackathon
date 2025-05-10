using HealthMed.Migrator.Data.Entities;
using HealthMed.Migrator.Data.Entities.Enum;
using HealthMed.QueryAPI.Controllers.Dtos.Doctor;
using HealthMed.QueryAPI.Controllers.Dtos.Doctor.Input;
using HealthMed.QueryAPI.Controllers.Dtos.Doctor.Output;
using HealthMed.QueryAPI.Interfaces.Services;
using HealthMed.QueryAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace HealthMed.QueryAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("doctor")]
    public class DoctorController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IDoctorService _doctorService;
        private readonly ILogger<DoctorController> _logger;

        public DoctorController(IUserService userService, IDoctorService doctorService, ILogger<DoctorController> logger)
        {
            _userService = userService;
            _doctorService = doctorService;
            _logger = logger;
        }

        [HttpGet]
        [Route("consultations")]
        public async Task<ActionResult<PaginationOutput<ConsultationDto>>> GetConsultations([FromQuery] GetDoctorConsultationsParams param)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                User doctor = null;
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    return StatusCode(401);
                }
                var tokenS = tokenHandler.ReadToken(token) as JwtSecurityToken;
                if (tokenS != null)
                {
                    var userName = tokenS.Claims.First(claim => claim.Type == "unique_name").Value;
                    if (!string.IsNullOrEmpty(userName))
                    {
                        doctor = await _userService.Get(userName);
                    }
                }
                if (doctor == null)
                {
                    return StatusCode(401);
                }
                if (doctor.Type != UserType.Doctor)
                {
                    return StatusCode(401);
                }
                if (doctor != null)
                {
                    var output = new PaginationOutput<ConsultationDto>();
                    var (consultations, total) = await _doctorService.ListMedicalConsultation(doctor.Id, param.PageSize, param.PageNumber, param.SortBy, param.SortDirection);
                    var consultationsDto = new List<ConsultationDto>();
                    foreach (var consultation in consultations)
                    {
                        var patient = await _userService.Get(consultation.PatientId);
                        consultationsDto.Add(new ConsultationDto()
                        {
                            PatientId = patient.Id,
                            Id = consultation.Id,
                            Justification = consultation.Justification,
                            PatientName = patient.Name,
                            ScheduledDate = consultation.ScheduledDate,
                            ScheduleTime = consultation.ScheduleTime,
                            Status = consultation.Status,
                            CreationTime = consultation.CreationTime,
                            UpdateTime = consultation.UpdateTime
                        });
                    }
                    output.PageSize = param.PageSize;
                    output.CurrentPage = param.PageNumber;
                    output.TotalPages = param.GetTotalPages(total);
                    output.TotalCount = total;
                    output.Value = consultationsDto;
                    return Ok(output);
                }
                else
                {
                    return NotFound("Médico não encontrado");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Ocorreu um erro inesperado" });
            }
        }

        [HttpGet]
        [Route("pendingConsultations")]
        public async Task<ActionResult<PaginationOutput<ConsultationDto>>> GetPendingConsultations([FromQuery] GetPendingConsultationsParams param)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                User doctor = null;
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    return StatusCode(401);
                }
                var tokenS = tokenHandler.ReadToken(token) as JwtSecurityToken;
                if (tokenS != null)
                {
                    var userName = tokenS.Claims.First(claim => claim.Type == "unique_name").Value;
                    if (!string.IsNullOrEmpty(userName))
                    {
                        doctor = await _userService.Get(userName);
                    }
                }
                if (doctor == null)
                {
                    return StatusCode(401);
                }
                if (doctor.Type != UserType.Doctor)
                {
                    return StatusCode(401);
                }
                if (doctor != null)
                {
                    var output = new PaginationOutput<ConsultationDto>();
                    var (consultations, total) = await _doctorService.ListPendingMedicalConsultation(doctor.Id, param.PageSize, param.PageNumber, param.SortBy, param.SortDirection);
                    var consultationsDto = new List<ConsultationDto>();
                    foreach (var consultation in consultations)
                    {
                        var patient = await _userService.Get(consultation.PatientId);
                        consultationsDto.Add(new ConsultationDto()
                        {
                            PatientId = patient.Id,
                            Id = consultation.Id,
                            Justification = consultation.Justification,
                            PatientName = patient.Name,
                            ScheduledDate = consultation.ScheduledDate,
                            ScheduleTime = consultation.ScheduleTime,
                            Status = consultation.Status,
                            CreationTime = consultation.CreationTime,
                            UpdateTime = consultation.UpdateTime
                        });
                    }
                    output.PageSize = param.PageSize;
                    output.CurrentPage = param.PageNumber;
                    output.TotalPages = param.GetTotalPages(total);
                    output.TotalCount = total;
                    output.Value = consultationsDto;
                    return Ok(output);
                }
                else
                {
                    return NotFound("Médico não encontrado");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Ocorreu um erro inesperado" });
            }
        }

        [HttpGet]
        [Route("{doctorId}")]
        public async Task<ActionResult<PaginationOutput<ScheduleInfoDto>>> GetDoctorSchedule([FromQuery] GetDoctorScheduleParams param, Guid doctorId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                User user = null;
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    return StatusCode(401);
                }
                var tokenS = tokenHandler.ReadToken(token) as JwtSecurityToken;
                if (tokenS != null)
                {
                    var userName = tokenS.Claims.First(claim => claim.Type == "unique_name").Value;
                    if (!string.IsNullOrEmpty(userName))
                    {
                        user = await _userService.Get(userName);
                    }
                }
                if (user == null)
                {
                    return StatusCode(401);
                }
                if (user.Type != UserType.Patient)
                {
                    return StatusCode(401);
                }

                if (user != null)
                {
                    var output = new PaginationOutput<ScheduleInfoDto>();
                    DateTime currentDate = DateTime.Now;
                    if (param.PageNumber > 1)
                    {
                        currentDate = currentDate.AddMonths(param.PageNumber);

                        currentDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                    }
                    DateTime lastDayOfMonth = currentDate.AddMonths(1).AddDays(-1);

                    var days = new List<DateTime>();
                    var doctorOffDays = await _doctorService.GetOffDays(doctorId);

                    for (DateTime day = currentDate; day <= lastDayOfMonth; day = day.AddDays(1))
                    {
                        if (!doctorOffDays.Any(x => x.OffDate == day))
                        {
                            days.Add(day.Date);
                        }
                    }
                    var doctorWorkDays = await _doctorService.GetWorkDays(doctorId);
                    var infos = new List<ScheduleInfoDto>();
                    TimeSpan nowTimeOnly = DateTime.Now.TimeOfDay;
                    foreach (var day in days)
                    {
                        if (doctorWorkDays.Any(x => x.DayOfWeek == day.DayOfWeek))
                        {
                            var workday = doctorWorkDays.FirstOrDefault(x => x.DayOfWeek == day.DayOfWeek);
                            if (workday != null)
                            {
                                if (workday.StartTime == null)
                                {
                                    workday.StartTime = new TimeSpan(8, 0, 0);
                                }
                                if (workday.EndTime == null)
                                {
                                    workday.EndTime = new TimeSpan(23, 0, 0);
                                }
                                if (workday.StartTime.HasValue && workday.EndTime.HasValue)
                                {
                                    var info = new ScheduleInfoDto();
                                    info.ScheduleDate = $"{day.ToString("dddd")} - {day.ToString("dd/MM/yyyy")}";
                                    var hours = new List<TimeSpan>();
                                    TimeSpan currentTime = workday.StartTime.Value;
                                    TimeSpan oneHour = TimeSpan.FromHours(1);
                                    while (currentTime <= workday.EndTime.Value)
                                    {
                                        if (day.Date == DateTime.Now.Date)
                                        {
                                            if (currentTime > nowTimeOnly)
                                            {
                                                hours.Add(currentTime);
                                            }
                                        }
                                        else
                                        {
                                            hours.Add(currentTime);
                                        }
                                        currentTime = currentTime.Add(oneHour);
                                    }
                                    info.ScheduleTimes = hours;
                                    info.DoctorName = user.Name;
                                    info.DoctorId = user.Id;
                                    infos.Add(info);
                                }
                            }
                        }
                    }
                    output.CurrentPage = param.PageNumber;
                    output.TotalCount = infos.Count();
                    output.Value = infos;
                    return Ok(output);
                }
                else
                {
                    return NotFound("Médico não encontrado");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Ocorreu um erro inesperado" });
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<PaginationOutput<DoctorDto>>> List([FromQuery] ListDoctorQueryParams param)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                User doctor = null;
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    return StatusCode(401);
                }
                var tokenS = tokenHandler.ReadToken(token) as JwtSecurityToken;
                if (tokenS != null)
                {
                    var userName = tokenS.Claims.First(claim => claim.Type == "unique_name").Value;
                    if (!string.IsNullOrEmpty(userName))
                    {
                        doctor = await _userService.Get(userName);
                    }
                }
                if (doctor == null)
                {
                    return StatusCode(401);
                }
                if (doctor.Type != UserType.Patient)
                {
                    return StatusCode(401);
                }

                var output = new PaginationOutput<DoctorDto>();
                var doctors = new List<DoctorDto>();
                var (users, total) = await _doctorService.GetAllDoctors(param.DoctorId, param.PageSize, param.PageNumber, param.SortBy, param.SortDirection);
                foreach (var user in users)
                {
                    doctors.Add(new DoctorDto()
                    {
                        Id = user.Id,
                        EmailAddress = user.EmailAddress,
                        Name = user.Name,
                        PhoneNumber = user.PhoneNumber,
                        Specialty = user.Specialty,
                    });
                }
                output.PageSize = param.PageSize;
                output.CurrentPage = param.PageNumber;
                output.TotalPages = param.GetTotalPages(total);
                output.TotalCount = total;
                output.Value = doctors;
                return Ok(output);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Ocorreu um erro inesperado" });
            }
        }
    }
}