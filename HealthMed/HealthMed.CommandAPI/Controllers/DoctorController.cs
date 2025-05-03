using HealthMed.CommandAPI.Controllers.Dtos.Doctor.Input;
using HealthMed.CommandAPI.Interfaces.Services;
using HealthMed.Migrator.Data.Entities.Enum;
using Microsoft.AspNetCore.Mvc;

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
        [Route("createSchedule")]
        public async Task<ActionResult> CreateSchedule(CreateScheduleInput input)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = await _userService.Get(input.DoctorId);
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