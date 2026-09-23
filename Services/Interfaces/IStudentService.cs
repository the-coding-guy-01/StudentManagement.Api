using StudentManagement.Api.DTOs;

namespace StudentManagement.Api.Services.Interfaces
{
    public interface IStudentService
    {
        Task<List<StudentDto>> GetAllAsync();

        Task<StudentDto?> GetByIdAsync(int id);

        Task<StudentDto> CreateAsync(CreateStudentDto dto);

        Task<StudentDto?> UpdateAsync(
            int id,
            UpdateStudentDto dto);

        Task<bool> DeleteAsync(int id);

        Task<int> GetTotalStudentsAsync();
    }
}