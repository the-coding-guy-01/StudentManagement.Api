using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.DTOs.Dashboard;
using StudentManagement.Api.Services.Interfaces;

namespace StudentManagement.Api.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<DashboardDto> GetDashboardAsync()
        {
            // Students
            var totalStudents = await _context.Students.CountAsync();

            var activeStudents = await _context.Students
                .CountAsync(s => s.IsActive);

            var inactiveStudents = totalStudents - activeStudents;


            // Teachers
            var totalTeachers = await _context.Teachers.CountAsync();


            // Courses
            var totalCourses = await _context.Courses.CountAsync();

            var activeCourses = await _context.Courses
                .CountAsync(c => c.IsActive);

            var inactiveCourses = totalCourses - activeCourses;


            // Latest 5 students
            var recentStudents = await _context.Students
                .AsNoTracking()
                .OrderByDescending(s => s.EnrollmentDate)
                .ThenByDescending(s => s.Id)
                .Take(5)
                .Select(s => new RecentStudentDto
                {
                    Id = s.Id,

                    AdmissionNumber = s.AdmissionNumber,

                    Name = s.FirstName + " " + s.LastName,

                    Email = s.Email,

                    Gender = s.Gender,

                    EnrollmentDate = s.EnrollmentDate,

                    IsActive = s.IsActive
                })
                .ToListAsync();


            return new DashboardDto
            {
                TotalStudents = totalStudents,

                TotalTeachers = totalTeachers,

                TotalCourses = totalCourses,

                ActiveStudents = activeStudents,

                InactiveStudents = inactiveStudents,

                ActiveCourses = activeCourses,

                InactiveCourses = inactiveCourses,

                RecentStudents = recentStudents
            };
        }
    }
}