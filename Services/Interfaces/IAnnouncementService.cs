using StudentManagement.Api.DTOs;

namespace StudentManagement.Api.Services.Interfaces
{
    public interface IAnnouncementService
    {
        Task<AnnouncementDto> GetAnnouncementByIdAsync(int id);
        Task<IEnumerable<AnnouncementDto>> GetAllAnnouncementsAsync();
        Task<AnnouncementDto> CreateAnnouncementAsync(CreateAnnouncementDto createAnnouncementDto);
        Task<AnnouncementDto> UpdateAnnouncementAsync(int id, UpdateAnnouncementDto updateAnnouncementDto);
        Task<bool> DeleteAnnouncementAsync(int id);
    }
}
