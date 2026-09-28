using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Api.DTOs.StudentPortal
{
    public class UpdateStudentProfileDto
    {
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;
    }
}