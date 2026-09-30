using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.DTOs.StudentPortal;
using StudentManagement.Api.Services.Interfaces;
using System.Security.Claims;

namespace StudentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/student/courses")]
    [Authorize(Roles = "Student")]
    public class StudentCoursesController
        : ControllerBase
    {
        private readonly IStudentCourseService
            _studentCourseService;

        public StudentCoursesController(
            IStudentCourseService studentCourseService)
        {
            _studentCourseService =
                studentCourseService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MyCourseDto>>>
            GetMyCourses()
        {
            var studentIdValue =
                User.FindFirstValue("StudentId");

            if (!int.TryParse(
                    studentIdValue,
                    out var studentId))
            {
                return Forbid();
            }

            var courses =
                await _studentCourseService
                    .GetMyCoursesAsync(studentId);

            if (courses == null || courses.Count == 0)
            {
                return NotFound(
                    new
                    {
                        message =
                            "No courses have been assigned to this student."
                    });
            }

            return Ok(courses);
        }
    }
}