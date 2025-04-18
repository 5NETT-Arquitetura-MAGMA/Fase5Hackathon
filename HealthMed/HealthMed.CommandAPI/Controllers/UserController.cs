using HealthMed.CommandAPI.Controllers.Dtos.User.Input;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.CommandAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public UserController()
        {
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(CreateUserInput input)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}