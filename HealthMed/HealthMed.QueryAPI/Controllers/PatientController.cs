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

        [HttpGet]
        [Route("consultations")]
        public async Task<ActionResult<PaginationOutput<ConsultationDto>>> GetConsultations([FromQuery] GetConsultationsParams param)
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
                    var output = new PaginationOutput<ConsultationDto>();
                    var (consultations, total) = await _doctorService.ListPatientMedicalConsultation(user.Id, param.PageSize, param.PageNumber, param.SortBy, param.SortDirection);
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
                    return NotFound("Paciente não encontrado");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Ocorreu um erro inesperado" });
            }
        }
    }
}