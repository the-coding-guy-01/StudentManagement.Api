using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.DTOs;
using StudentManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace StudentManagement.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementsController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementsController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAnnouncement(int id)
        {
            try
            {
                var announcement = await _announcementService.GetAnnouncementByIdAsync(id);
                return Ok(announcement);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnnouncements()
        {
            var announcements = await _announcementService.GetAllAnnouncementsAsync();
            return Ok(announcements);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnnouncement([FromBody] CreateAnnouncementDto createAnnouncementDto)
        {
            var announcement = await _announcementService.CreateAnnouncementAsync(createAnnouncementDto);
            return CreatedAtAction(nameof(GetAnnouncement), new { id = announcement.Id }, announcement);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAnnouncement(int id, [FromBody] UpdateAnnouncementDto updateAnnouncementDto)
        {
            try
            {
                var announcement = await _announcementService.UpdateAnnouncementAsync(id, updateAnnouncementDto);
                return Ok(announcement);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            try
            {
                await _announcementService.DeleteAnnouncementAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
