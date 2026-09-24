using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Api.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string CourseCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string CourseName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public int Credits { get; set; }

        public int Duration { get; set; }

        public bool IsActive { get; set; } = true;

        // Foreign Key
        public int? TeacherId { get; set; }

        // Navigation Property
        public Teacher? Teacher { get; set; }

        public ICollection<Student> Students { get; set; }
            = new List<Student>();
    }
}