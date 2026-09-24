using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Api.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        // Used later for the Student portal
        public int? StudentId { get; set; }

        // Used later for the Teacher portal
        public int? TeacherId { get; set; }
    }
}