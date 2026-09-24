namespace StudentManagement.Api.DTOs
{
    public class StudentDto
    {
        public int Id { get; set; }

        public string AdmissionNumber { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        public string Gender { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public DateTime EnrollmentDate { get; set; }

        public bool IsActive { get; set; }

        public int? CourseId { get; set; }

        public string? CourseName { get; set; }
    }
}