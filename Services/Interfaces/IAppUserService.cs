using StudentManagement.Api.DTOs.AppUsers;

namespace StudentManagement.Api.Services.Interfaces
{
    public interface IAppUserService
    {
        Task<bool> CreateUserAsync(CreateUserDto request);

        Task<bool> RegisterStudentAsync(
            RegisterStudentUserDto request);

        Task<List<int>> GetRegisteredStudentIdsAsync();
    }
}
