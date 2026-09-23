using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.DTOs;
using StudentManagement.Api.Services.Interfaces;

namespace StudentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(
            IStudentService studentService)
        {
            _studentService = studentService;
        }


        [HttpGet]
        public async Task<ActionResult<List<StudentDto>>> GetAll()
        {
            var students =
                await _studentService.GetAllAsync();

            return Ok(students);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<StudentDto>> GetById(int id)
        {
            var student =
                await _studentService.GetByIdAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetTotalStudents()
        {
            var total = await _studentService.GetTotalStudentsAsync();

            return Ok(total);
        }


        [HttpPost]
        public async Task<ActionResult<StudentDto>> Create(
            CreateStudentDto dto)
        {
            var student =
                await _studentService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = student.Id },
                student);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<StudentDto>> Update(
            int id,
            UpdateStudentDto dto)
        {
            var student =
                await _studentService.UpdateAsync(id, dto);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted =
                await _studentService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}