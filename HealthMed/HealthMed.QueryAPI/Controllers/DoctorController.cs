using HealthMed.QueryAPI.Controllers.Dtos.Doctor;
using HealthMed.QueryAPI.Controllers.Dtos.Doctor.Input;
using HealthMed.QueryAPI.Controllers.Dtos.Doctor.Output;
using HealthMed.QueryAPI.Interfaces.Services;
using HealthMed.QueryAPI.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HealthMed.QueryAPI.Controllers
{
    [ApiController]
    [Route("doctor")]
    public class DoctorController : ControllerBase
    {
        private readonly IUserService _userService;

        public DoctorController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<PaginationOutput<DoctorDto>>> List([FromQuery] DoctorQueryParams param)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var output = new PaginationOutput<DoctorDto>();
                var doctors = new List<DoctorDto>();
                var (users, total) = await _userService.GetAllDoctors(param.DoctorId, param.PageSize, param.PageNumber, param.SortBy, param.SortDirection);
                foreach (var user in users)
                {
                    doctors.Add(new DoctorDto()
                    {
                        Id = user.Id,
                        EmailAddress = user.EmailAddress,
                        Name = user.Name,
                        PhoneNumber = user.PhoneNumber,
                        Specialty = user.Specialty,
                    });
                }
                output.PageSize = param.PageSize;
                output.CurrentPage = param.PageNumber;
                output.TotalPages = param.GetTotalPages(total);
                output.TotalCount = total;
                output.Value = doctors;
                return Ok(output);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}