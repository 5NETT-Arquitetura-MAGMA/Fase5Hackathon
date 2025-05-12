using HealthMed.CommandAPI.Controllers.Dtos.Patient.Input;
using HealthMed.CommandAPI.Interfaces.Services;
using HealthMed.Migrator.Data.Entities;
using HealthMed.Migrator.Data.Entities.Enum;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

namespace HealthMed.CommandAPI.Controllers
{
    [ApiController]
    [Route("patient")]
    public class PatientController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IDoctorService _doctorService;
        private readonly ILogger<PatientController> _logger;

        public PatientController(IUserService userService, IDoctorService doctorService, ILogger<PatientController> logger)
        {
            _userService = userService;
            _doctorService = doctorService;
            _logger = logger;
        }

        [HttpPost]
        [Route("cancelConsultation")]
        public async Task<ActionResult> CancelConsultation(CancelConsultationInput input)
        {
            try
            {
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
                var consultation = await _doctorService.GetConsultation(input.ConsultationId);
                if (consultation == null)
                {
                    return NotFound(new { message = "Consulta não encontrada" });
                }
                if (consultation.PatientId != user.Id)
                {
                    return StatusCode(401);
                }
                await _doctorService.UpdateConsultation(input.ConsultationId, ConsultationStatus.Canceled, input.Justification);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Ocorreu um erro inesperado" });
            }
        }

        [HttpPost]
        [Route("scheduleConsultation")]
        public async Task<ActionResult> ScheduleConsultation(ScheduleConsultationInput input)
        {
            try
            {
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
                var scheduleDateConv = ConvertStringToDateTime(input.ScheduleDate);
                if (!scheduleDateConv.HasValue)
                {
                    return BadRequest(new { message = "\"ScheduleDate\" não pode ser nula ou vazia" });
                }
                var scheduleDate = scheduleDateConv.Value;

                var doctorWorkDays = await _doctorService.GetWorkDays(input.DoctorId);
                if (doctorWorkDays != null && !doctorWorkDays.Any(x => x.DayOfWeek == scheduleDate.DayOfWeek))
                {
                    return BadRequest(new { message = "Médico não disponivel na data solicitada" });
                }

                var doctorOffDays = await _doctorService.GetOffDays(input.DoctorId);
                if (doctorOffDays != null && doctorOffDays.Any(x => x.OffDate.Date == scheduleDate.Date))
                {
                    return BadRequest(new { message = "Médico não disponivel na data solicitada" });
                }

                var consultations = await _doctorService.GetConsultations(input.DoctorId);
                if (consultations != null)
                {
                    if (consultations.Any(x => (x.Status == ConsultationStatus.PendingConfirmation || x.Status == ConsultationStatus.Confirmed) && x.ScheduledDate.Date == scheduleDate.Date && x.ScheduleTime == input.ScheduleTime))
                    {
                        if (consultations.Any(x => x.PatientId == user.Id && (x.Status == ConsultationStatus.PendingConfirmation || x.Status == ConsultationStatus.Confirmed) && x.ScheduledDate.Date == scheduleDate.Date && x.ScheduleTime == input.ScheduleTime))
                            return BadRequest(new { message = "Consulta já agendada ou aguardando confirmação" });
                        else
                            return BadRequest(new { message = "Médico não disponivel na data solicitada" });
                    }
                    else if (consultations.Any(x => x.PatientId == user.Id && x.Status == ConsultationStatus.Rejected && x.ScheduledDate.Date == scheduleDate.Date && x.ScheduleTime == input.ScheduleTime))
                        return BadRequest(new { message = "Médico não disponivel na data solicitada" });
                }
                var consultation = await _doctorService.CreateConsultation(user.Id, input.DoctorId, scheduleDate, input.ScheduleTime);

                return StatusCode(201, new { consultation.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Ocorreu um erro inesperado" });
            }
        }

        #region Private Methods

        private static DateTime? ConvertStringToDateTime(string dateStr)
        {
            if (!string.IsNullOrEmpty(dateStr))
            {
                DateTime result;
                if (DateTime.TryParseExact(dateStr, new string[] { "dd/MM/yy", "dd/MM/yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                {
                    return result;
                }
            }

            return null;
        }

        #endregion Private Methods
    }
}