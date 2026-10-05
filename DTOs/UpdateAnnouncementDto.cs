namespace StudentManagement.Api.DTOs
{
    public class UpdateAnnouncementDto
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
