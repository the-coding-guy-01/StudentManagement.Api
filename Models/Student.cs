using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Api.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string AdmissionNumber { get; set; } = string.Empty;

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

        public DateTime EnrollmentDate { get; set; }

        //Thgis is a test comment
        public bool IsActive { get; set; } = true;
    }
}