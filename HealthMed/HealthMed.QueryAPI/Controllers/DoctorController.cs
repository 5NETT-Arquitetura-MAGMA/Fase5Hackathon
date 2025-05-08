using HealthMed.QueryAPI.Controllers.Dtos.Doctor;
using HealthMed.QueryAPI.Controllers.Dtos.Doctor.Input;
using HealthMed.QueryAPI.Controllers.Dtos.Doctor.Output;
using HealthMed.QueryAPI.Interfaces.Services;
using HealthMed.QueryAPI.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.QueryAPI.Controllers
{
    [ApiController]
    [Route("doctor")]
    public class DoctorController : ControllerBase
    {
        private readonly IUserService _service;

        public DoctorController(IUserService service)
        {
            _service = service;
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
                var doctor = await _service.Get(doctorId);
                if (doctor != null)
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
                    var doctorOffDays = await _service.GetOffDays(doctorId);

                    for (DateTime day = currentDate; day <= lastDayOfMonth; day = day.AddDays(1))
                    {
                        if (!doctorOffDays.Any(x => x.OffDate == day))
                        {
                            days.Add(day.Date);
                        }
                    }
                    var doctorWorkDays = await _service.GetWorkDays(doctorId);
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
                                    info.DoctorName = doctor.Name;
                                    info.DoctorId = doctor.Id;
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
                throw;
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<PaginationOutput<DoctorDto>>> List([FromQuery] DoctorQueryParams param)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var output = new PaginationOutput<DoctorDto>();
                var doctors = new List<DoctorDto>();
                var (users, total) = await _service.GetAllDoctors(param.DoctorId, param.PageSize, param.PageNumber, param.SortBy, param.SortDirection);
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
                throw;
            }
        }
    }
}