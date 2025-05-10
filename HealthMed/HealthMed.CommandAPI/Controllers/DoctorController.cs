using HealthMed.CommandAPI.Controllers.Dtos.Doctor.Input;
using HealthMed.CommandAPI.Interfaces.Services;
using HealthMed.Migrator.Data.Entities;
using HealthMed.Migrator.Data.Entities.Enum;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace HealthMed.CommandAPI.Controllers
{
    [ApiController]
    [Route("doctor")]
    public class DoctorController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IDoctorService _doctorService;

        public DoctorController(IUserService userService, IDoctorService doctorService)
        {
            _userService = userService;
            _doctorService = doctorService;
        }

        [HttpPost]
        [Route("updateConsultation")]
        public async Task<ActionResult> UpdateConsultation(UpdateConsultationInput input)
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
                if (user.Type != UserType.Doctor)
                {
                    return StatusCode(401);
                }
                var consultation = await _doctorService.GetConsultation(input.ConsultationId);
                if (consultation == null)
                {
                    return NotFound(new { message = "Consulta não encontrada" });
                }
                if (consultation.DoctorId != user.Id)
                {
                    return StatusCode(401);
                }
                await _doctorService.UpdateConsultation(input.ConsultationId, input.Accepted, input.Justification);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("createOffDay")]
        public async Task<ActionResult> CreateOffDay(CreateOffDayInput input)
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
                if (user.Type != UserType.Doctor)
                {
                    return StatusCode(401);
                }

                if (user != null && Guid.Empty != user.Id && user.Type == UserType.Doctor)
                {
                    await _doctorService.CreateOffDay(input.DoctorId, input.OffDate);
                    return Created();
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

        [HttpPost]
        [Route("createSchedule")]
        public async Task<ActionResult> CreateSchedule(CreateScheduleInput input)
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
                if (user.Type != UserType.Doctor)
                {
                    return StatusCode(401);
                }
                if (user != null && Guid.Empty != user.Id && user.Type == UserType.Doctor)
                {
                    await _doctorService.CreateSchedule(input.DoctorId, input.DayOfWeek, input.StartTime, input.EndTime);
                    return Created();
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
    }
}