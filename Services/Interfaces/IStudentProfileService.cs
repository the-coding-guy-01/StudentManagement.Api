using Microsoft.AspNetCore.Http;
using StudentManagement.Api.DTOs.StudentPortal;

namespace StudentManagement.Api.Services.Interfaces
{
    public interface IStudentProfileService
    {
        Task<StudentProfileDto?> GetProfileAsync(
            int studentId);

        Task<StudentProfileDto?> UpdateProfileAsync(
            int studentId,
            UpdateStudentProfileDto dto);

        Task<StudentProfileDto?> UploadProfileImageAsync(
            int studentId,
            IFormFile file);

        Task<bool> RemoveProfileImageAsync(
            int studentId);
    }
}