using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.Services.Interfaces;
using StudentManagement.Shared.DTOs;

namespace StudentManagement.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : Controller
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<List<CourseDto>>> GetAll()
        {
            var courses = await _courseService.GetAllAsync();
            return Ok(courses);
        }

        // GET: api/Courses/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CourseDto>> GetById(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        // POST: api/Courses
        [HttpPost]
        public async Task<ActionResult<CourseDto>> Create(CreateCourseDto dto)
        {
            var course = await _courseService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
        }

        // PUT: api/Courses/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateCourseDto dto)
        {
            var success = await _courseService.UpdateAsync(id, dto);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{courseId:int}/teacher/{teacherId:int}")]
        public async Task<ActionResult<CourseDto>> AssignTeacher(
            int courseId,
            int teacherId)
        {
            var course =
                await _courseService.AssignTeacherAsync(
                    courseId,
                    teacherId);


            if (course == null)
            {
                return NotFound();
            }


            return Ok(course);
        }


        [HttpDelete("{courseId:int}/teacher")]
        public async Task<IActionResult> RemoveTeacher(
            int courseId)
        {
            var removed =
                await _courseService.RemoveTeacherAsync(
                    courseId);


            if (!removed)
            {
                return NotFound();
            }


            return NoContent();
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _courseService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
