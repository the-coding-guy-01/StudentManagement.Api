namespace StudentManagement.Api.DTOs
{
    public class TeacherDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        public string Gender { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public DateTime JoinDate { get; set; }

        public bool IsActive { get; set; }
    }
}