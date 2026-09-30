using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.DTOs.StudentPortal;
using StudentManagement.Api.Services.Interfaces;

namespace StudentManagement.Api.Services
{
    public class StudentCourseService
        : IStudentCourseService
    {
        private readonly ApplicationDbContext _context;

        public StudentCourseService(
            ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<MyCourseDto>>
            GetMyCoursesAsync(int studentId)
        {
            var student = await _context.Students
                .AsNoTracking()
                .Include(x => x.StudentCourses)
                    .ThenInclude(x => x.Course)
                        .ThenInclude(x => x!.Teacher)
                .FirstOrDefaultAsync(x => x.Id == studentId);

            if (student == null)
            {
                return new List<MyCourseDto>();
            }

            return student.StudentCourses
                .Where(x => x.Course != null)
                .Select(x => new MyCourseDto
                {
                    CourseId = x.Course!.Id,

                    CourseCode = x.Course.CourseCode,

                    CourseName = x.Course.CourseName,

                    Description = x.Course.Description,

                    Credits = x.Course.Credits,

                    Duration = x.Course.Duration,

                    IsActive = x.Course.IsActive,

                    TeacherName = x.Course.Teacher == null
                        ? null
                        : $"{x.Course.Teacher.FirstName} {x.Course.Teacher.LastName}"
                })
                .ToList();
        }
    }
}