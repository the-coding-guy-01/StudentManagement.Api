using StudentManagement.Api.DTOs;

namespace StudentManagement.Api.Services.Interfaces
{
    public interface ITeacherService
    {
        Task<List<TeacherDto>> GetAllAsync();

        Task<TeacherDto?> GetByIdAsync(int id);

        Task<TeacherDto> CreateAsync(CreateTeacherDto dto);

        Task<TeacherDto?> UpdateAsync(
            int id,
            UpdateTeacherDto dto);

        Task<bool> DeleteAsync(int id);

        Task<int> GetTotalTeachersAsync();
    }
}