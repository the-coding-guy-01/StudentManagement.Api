namespace StudentManagement.Api.DTOs.StudentPortal
{
    public class MyCourseDto
    {
        public int CourseId { get; set; }

        public string CourseCode { get; set; }
            = string.Empty;

        public string CourseName { get; set; }
            = string.Empty;

        public string? Description { get; set; }

        public int Credits { get; set; }

        public int Duration { get; set; }

        public bool IsActive { get; set; }

        public string? TeacherName { get; set; }
    }
}