using HealthMed.CommandAPI.Controllers.Dtos.Patient.Input;
using HealthMed.CommandAPI.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.CommandAPI.Controllers
{
    [ApiController]
    [Route("patient")]
    public class PatientController : ControllerBase
    {
        private readonly IUserService _userService;

        public PatientController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("schedulecCnsultation")]
        public async Task<ActionResult> SchedulecCnsultation(SchedulecCnsultationInput input)
        {
            return Ok();
        }
    }
}