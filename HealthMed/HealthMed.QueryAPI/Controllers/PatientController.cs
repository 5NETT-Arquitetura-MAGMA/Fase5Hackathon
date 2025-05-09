using HealthMed.QueryAPI.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.QueryAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("patient")]
    public class PatientController : ControllerBase
    {
        private readonly IUserService _userService;

        public PatientController(IUserService userService)
        {
            _userService = userService;
        }
    }
}