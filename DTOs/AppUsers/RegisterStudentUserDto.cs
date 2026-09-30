using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Api.DTOs.AppUsers
{
    public class RegisterStudentUserDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}