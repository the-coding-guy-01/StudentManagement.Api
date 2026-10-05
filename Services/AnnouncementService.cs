using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.DTOs;
using StudentManagement.Api.Models;
using StudentManagement.Api.Services.Interfaces;

namespace StudentManagement.Api.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly ApplicationDbContext _context;
        public AnnouncementService(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<AnnouncementDto> GetAnnouncementByIdAsync(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
            {
                throw new KeyNotFoundException($"Announcement with ID {id} not found.");
            }
            return new AnnouncementDto
            {
                Title = announcement.Title,
                Message = announcement.Message,
                CreatedAt = announcement.CreatedAt,
                UpdatedAt = announcement.UpdatedAt,
                IsActive = announcement.IsActive
            };
        }

        public async Task<IEnumerable<AnnouncementDto>> GetAllAnnouncementsAsync()
        {
            var announcements = await _context.Announcements.ToListAsync();
            return announcements.Select(a => new AnnouncementDto
            {
                Title = a.Title,
                Message = a.Message,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt,
                IsActive = a.IsActive
            });
        }

        // Create announcement
        public async Task<AnnouncementDto> CreateAnnouncementAsync(CreateAnnouncementDto createAnnouncementDto)
        {
            var announcement = new Announcement
            {
                Title = createAnnouncementDto.Title,
                Message = createAnnouncementDto.Message,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };
            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();
            return new AnnouncementDto
            {
                Title = announcement.Title,
                Message = announcement.Message,
                CreatedAt = announcement.CreatedAt,
                UpdatedAt = announcement.UpdatedAt,
                IsActive = announcement.IsActive
            };
        }

        public async Task<AnnouncementDto> UpdateAnnouncementAsync(int id, UpdateAnnouncementDto updateAnnouncementDto)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
            {
                throw new KeyNotFoundException($"Announcement with ID {id} not found.");
            }
            announcement.Title = updateAnnouncementDto.Title;
            announcement.Message = updateAnnouncementDto.Message;
            announcement.IsActive = updateAnnouncementDto.IsActive;
            announcement.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return new AnnouncementDto
            {
                Title = announcement.Title,
                Message = announcement.Message,
                CreatedAt = announcement.CreatedAt,
                UpdatedAt = announcement.UpdatedAt,
                IsActive = announcement.IsActive
            };
        }

        public async Task<bool> DeleteAnnouncementAsync(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return false;
            }
            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
