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


        public async Task<MyCourseDto?>
            GetMyCourseAsync(int studentId)
        {
            var student = await _context.Students
                .AsNoTracking()
                .Include(x => x.Course)
                    .ThenInclude(x => x!.Teacher)
                .FirstOrDefaultAsync(
                    x => x.Id == studentId);


            if (student == null ||
                student.Course == null)
            {
                return null;
            }


            var course = student.Course;


            return new MyCourseDto
            {
                CourseId = course.Id,

                CourseCode = course.CourseCode,

                CourseName = course.CourseName,

                Description = course.Description,

                Credits = course.Credits,

                Duration = course.Duration,

                IsActive = course.IsActive,

                TeacherName =
                    course.Teacher == null
                        ? null
                        : $"{course.Teacher.FirstName} " +
                          $"{course.Teacher.LastName}"
            };
        }
    }
}