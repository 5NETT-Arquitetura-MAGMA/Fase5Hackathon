using HealthMed.CommandAPI.Controllers.Dtos.Doctor.Input;
using HealthMed.CommandAPI.Controllers.Dtos.Patient.Input;
using HealthMed.CommandAPI.Interfaces.Services;
using HealthMed.Migrator.Data.Entities;
using HealthMed.Migrator.Data.Entities.Enum;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace HealthMed.CommandAPI.Controllers
{
    [ApiController]
    [Route("patient")]
    public class PatientController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IDoctorService _doctorService;

        public PatientController(IUserService userService, IDoctorService doctorService)
        {
            _userService = userService;
            _doctorService = doctorService;
        }

        [HttpPost]
        [Route("CancelConsultation")]
        public async Task<ActionResult> CancelConsultation(UpdateConsultationInput input)
        {
            try
            {
                User user = null;
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var tokenS = tokenHandler.ReadToken(token) as JwtSecurityToken;
                if (tokenS != null)
                {
                    var userName = tokenS.Claims.First(claim => claim.Type == "userName").Value;
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
                await _doctorService.UpdateConsultation(input.ConsultationId, ConsultationStatus.Canceled, input.Justification);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
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
                var tokenS = tokenHandler.ReadToken(token) as JwtSecurityToken;
                if (tokenS != null)
                {
                    var userName = tokenS.Claims.First(claim => claim.Type == "userName").Value;
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

                var doctorWorkDays = await _doctorService.GetWorkDays(input.DoctorId);
                if (doctorWorkDays != null && doctorWorkDays.Any(x => x.DayOfWeek == input.ScheduleDate.DayOfWeek))
                {
                    return BadRequest(new { message = "Médico não disponivel na data solicitada" });
                }

                var doctorOffDays = await _doctorService.GetOffDays(input.DoctorId);
                if (doctorOffDays != null && doctorOffDays.Any(x => x.OffDate.Date == input.ScheduleDate.Date))
                {
                    return BadRequest(new { message = "Médico não disponivel na data solicitada" });
                }

                var consultations = await _doctorService.GetConsultations(input.DoctorId);
                if (consultations != null && consultations.Any(x => (x.Status == ConsultationStatus.PendingConfirmation || x.Status == ConsultationStatus.Confirmed) && x.ScheduledDate.Date == input.ScheduleDate.Date && x.ScheduleTime == input.ScheduleTime))
                {
                    if (consultations.Any(x => x.PatientId == user.Id && (x.Status == ConsultationStatus.PendingConfirmation || x.Status == ConsultationStatus.Confirmed) && x.ScheduledDate.Date == input.ScheduleDate.Date && x.ScheduleTime == input.ScheduleTime))
                        return BadRequest(new { message = "Consulta já agendada ou aguardando confirmação" });
                    else
                        return BadRequest(new { message = "Médico não disponivel na data solicitada" });
                }
                await _doctorService.CreateConsultation(user.Id, input.DoctorId, input.ScheduleDate, input.ScheduleTime);

                return Created();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}