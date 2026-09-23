using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Api.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(20)]
        public string Gender { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        public DateTime JoinDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Property
        public ICollection<Course> Courses { get; set; }
            = new List<Course>();
    }
}