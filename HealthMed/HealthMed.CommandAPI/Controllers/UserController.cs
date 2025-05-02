using HealthMed.CommandAPI.Controllers.Dtos.User.Input;
using HealthMed.CommandAPI.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.CommandAPI.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

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

                await _userService.CreateUser(input.Name, input.PhoneNumber, input.EmailAddress, input.Login, input.Password, null);

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

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

                await _userService.CreateUser(input.Name, input.PhoneNumber, input.EmailAddress, input.Login, input.Password, input.Specialty);

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}