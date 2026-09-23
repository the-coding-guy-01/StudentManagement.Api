namespace StudentManagement.Api.DTOs.Dashboard
{
    public class RecentStudentDto
    {
        public int Id { get; set; }

        public string AdmissionNumber { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public DateTime EnrollmentDate { get; set; }

        public bool IsActive { get; set; }
    }
}