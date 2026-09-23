using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Shared.DTOs
{
    public class CreateCourseDto
    {
        [Required]
        [MaxLength(100)]
        public string CourseName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Range(1, 20)]
        public int Credits { get; set; }

        [Range(1, 120)]
        public int Duration { get; set; }

        public int? TeacherId { get; set; }

    }
}