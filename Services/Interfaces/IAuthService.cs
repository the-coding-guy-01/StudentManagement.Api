using StudentManagement.Api.DTOs.Auth;

namespace StudentManagement.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(
            LoginRequestDto request);
    }
}