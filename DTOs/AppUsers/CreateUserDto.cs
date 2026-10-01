using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Api.DTOs.AppUsers
{
    public class CreateUserDto
    {
        [Required]
        [RegularExpression("^(Student|Teacher)$")]
        public string Role { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }
        [MaxLength(20)] public string Gender { get; set; } = string.Empty;
        [MaxLength(20)] public string Phone { get; set; } = string.Empty;
        public DateTime? EnrollmentDate { get; set; }
        public DateTime? JoinDate { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required, Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
