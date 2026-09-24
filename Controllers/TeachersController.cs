using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.DTOs;
using StudentManagement.Api.Services.Interfaces;

namespace StudentManagement.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeachersController(
            ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }


        [HttpGet]
        public async Task<ActionResult<List<TeacherDto>>> GetAll()
        {
            var teachers =
                await _teacherService.GetAllAsync();

            return Ok(teachers);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<TeacherDto>> GetById(int id)
        {
            var teacher =
                await _teacherService.GetByIdAsync(id);

            if (teacher == null)
            {
                return NotFound();
            }

            return Ok(teacher);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetTotalTeachers()
        {
            var total = await _teacherService.GetTotalTeachersAsync();

            return Ok(total);
        }


        [HttpPost]
        public async Task<ActionResult<TeacherDto>> Create(
            CreateTeacherDto dto)
        {
            var teacher =
                await _teacherService.CreateAsync(dto);
            return CreatedAtAction(
                nameof(GetById),
                new { id = teacher.Id },
                teacher);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<TeacherDto>> Update(
            int id,
            UpdateTeacherDto dto)
        {
            var teacher =
                await _teacherService.UpdateAsync(id, dto);

            if (teacher == null)
            {
                return NotFound();
            }

            return Ok(teacher);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted =
                await _teacherService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}