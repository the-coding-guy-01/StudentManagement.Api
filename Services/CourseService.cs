using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.Models;
using StudentManagement.Api.Services.Interfaces;
using StudentManagement.Shared.DTOs;

namespace StudentManagement.Api.Services
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext _context;

        public CourseService(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET ALL COURSES
        public async Task<List<CourseDto>> GetAllAsync()
        {
            var courses = await _context.Courses
                .Include(c => c.Teacher)
                .ToListAsync();

            return courses
                .Select(MapToDto)
                .ToList();
        }


        // GET COURSE BY ID
        public async Task<CourseDto?> GetByIdAsync(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return null;
            }

            return MapToDto(course);
        }


        // CREATE COURSE
        public async Task<CourseDto> CreateAsync(CreateCourseDto dto)
        {

            if (dto.TeacherId.HasValue)
            {
                var teacherExists = await _context.Teachers
                    .AnyAsync(t => t.Id == dto.TeacherId.Value);

                if (!teacherExists)
                {
                    throw new ArgumentException("Teacher not found.");
                }
            }

            var course = new Course
            {
                CourseCode = await GenerateCourseCodeAsync(),

                CourseName = dto.CourseName,

                Description = dto.Description,

                Credits = dto.Credits,

                Duration = dto.Duration,

                TeacherId = dto.TeacherId,

                IsActive = true
            };

            _context.Courses.Add(course);

            await _context.SaveChangesAsync();

            // Load teacher after saving
            if (course.TeacherId.HasValue)
            {
                await _context.Entry(course)
                    .Reference(c => c.Teacher)
                    .LoadAsync();
            }

            return MapToDto(course);
        }


        // UPDATE COURSE
        public async Task<bool> UpdateAsync(
            int id,
            UpdateCourseDto dto)
        {
            var course = await _context.Courses
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return false;
            }

            if (dto.TeacherId.HasValue)
            {
                var teacherExists = await _context.Teachers
                    .AnyAsync(t => t.Id == dto.TeacherId.Value);

                if (!teacherExists)
                {
                    throw new ArgumentException("Teacher not found.");
                }
            }

            course.CourseName = dto.CourseName;

            course.Description = dto.Description;

            course.Credits = dto.Credits;

            course.Duration = dto.Duration;

            course.IsActive = dto.IsActive;

            course.TeacherId = dto.TeacherId;

            await _context.SaveChangesAsync();

            return true;
        }


        // DELETE COURSE
        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return false;
            }

            _context.Courses.Remove(course);

            await _context.SaveChangesAsync();

            return true;
        }


        // GENERATE COURSE CODE
        private async Task<string> GenerateCourseCodeAsync()
        {
            var lastCourse = await _context.Courses
                .OrderByDescending(c => c.Id)
                .FirstOrDefaultAsync();

            int nextNumber = 1;

            if (lastCourse != null)
            {
                nextNumber = lastCourse.Id + 1;
            }

            return $"C{nextNumber:D3}";
        }

        public async Task<CourseDto?> AssignTeacherAsync(
            int courseId,
            int teacherId)
        {
            var course = await _context.Courses
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(c => c.Id == courseId);


            if (course == null)
            {
                return null;
            }


            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(t => t.Id == teacherId);


            if (teacher == null)
            {
                return null;
            }


            course.TeacherId = teacherId;


            await _context.SaveChangesAsync();


            course.Teacher = teacher;


            return MapToDto(course);
        }

        public async Task<bool> RemoveTeacherAsync(
            int courseId)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == courseId);


            if (course == null)
            {
                return false;
            }


            course.TeacherId = null;


            await _context.SaveChangesAsync();


            return true;
        }


        // MAP COURSE MODEL TO DTO
        private static CourseDto MapToDto(Course course)
        {
            return new CourseDto
            {
                Id = course.Id,

                CourseCode = course.CourseCode,

                CourseName = course.CourseName,

                Description = course.Description,

                Credits = course.Credits,

                Duration = course.Duration,

                IsActive = course.IsActive,

                TeacherId = course.TeacherId,

                TeacherName = course.Teacher == null
                    ? null
                    : $"{course.Teacher.FirstName} {course.Teacher.LastName}"
            };
        }
    }
}