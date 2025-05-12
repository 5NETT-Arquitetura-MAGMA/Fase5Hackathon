using Microsoft.AspNetCore.Mvc;

namespace HealthMed.QueryAPI.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [Route("ready")]
        public ActionResult Ready()
        {
            return Ok();
        }

        [HttpGet]
        [Route("live")]
        public ActionResult Live()
        {
            return Ok();
        }
    }
}