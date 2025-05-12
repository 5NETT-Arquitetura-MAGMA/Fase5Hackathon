using HealthMed.CommandAPI.Controllers.Dtos.User.Input;
using HealthMed.CommandAPI.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.CommandAPI.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("createPatientUser")]
        public async Task<ActionResult> CreatePatientUser(CreatePatientUserInput input)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var exists = await _userService.GetUserByCPF(input.Login);
                if (exists != null)
                {
                    return BadRequest(new { message = "Paciente ja existe em nossa base de dados" });
                }
                else
                {
                    await _userService.CreateUser(input.Name, input.PhoneNumber, input.EmailAddress, input.Login, input.Password, null);

                    return Created();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "Ocorreu um erro inesperado" });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("createMedicUser")]
        public async Task<ActionResult> CreateMedicUser(CreateMedicUserInput input)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var exists = await _userService.GetDoctorByCRM(input.Login);
                if (exists != null)
                {
                    return BadRequest(new { message = "Medico ja existe em nossa base de dados" });
                }
                else
                {
                    await _userService.CreateUser(input.Name, input.PhoneNumber, input.EmailAddress, input.Login, input.Password, input.Specialty);

                    return Created();
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