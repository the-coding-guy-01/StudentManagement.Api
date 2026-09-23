using StudentManagement.Shared.DTOs;

namespace StudentManagement.Api.Services.Interfaces
{
    public interface ICourseService
    {
        Task<List<CourseDto>> GetAllAsync();

        Task<CourseDto?> GetByIdAsync(int id);

        Task<CourseDto> CreateAsync(CreateCourseDto dto);

        Task<bool> UpdateAsync(int id, UpdateCourseDto dto);

        Task<bool> DeleteAsync(int id);

        Task<CourseDto?> AssignTeacherAsync(
            int courseId,
            int teacherId);

        Task<bool> RemoveTeacherAsync(
            int courseId);
    }
}