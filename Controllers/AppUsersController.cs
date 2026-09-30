using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.DTOs.AppUsers;
using StudentManagement.Api.Services.Interfaces;

namespace StudentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AppUsersController : ControllerBase
    {
        private readonly IAppUserService _appUserService;


        public AppUsersController(
            IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }


        [HttpPost("register-student")]
        public async Task<IActionResult> RegisterStudent(
            RegisterStudentUserDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var registered =
                await _appUserService
                    .RegisterStudentAsync(request);


            if (!registered)
            {
                return BadRequest(new
                {
                    message =
                        "Student could not be registered. " +
                        "The student may already have a login account."
                });
            }


            return Ok(new
            {
                message =
                    "Student login account registered successfully."
            });
        }

        [HttpGet("registered-student-ids")]
        public async Task<ActionResult<List<int>>> GetRegisteredStudentIds()
        {
            var studentIds =
                await _appUserService
                    .GetRegisteredStudentIdsAsync();

            return Ok(studentIds);
        }
    }
}