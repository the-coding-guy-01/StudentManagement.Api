using StudentManagement.Api.DTOs.StudentPortal;

namespace StudentManagement.Api.Services.Interfaces
{
    public interface IStudentCourseService
    {
        Task<MyCourseDto?> GetMyCourseAsync(
            int studentId);
    }
}