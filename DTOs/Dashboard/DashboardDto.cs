namespace StudentManagement.Api.DTOs.Dashboard
{
    public class DashboardDto
    {
        public int TotalStudents { get; set; }

        public int TotalTeachers { get; set; }

        public int TotalCourses { get; set; }

        public int ActiveStudents { get; set; }

        public int InactiveStudents { get; set; }

        public int ActiveCourses { get; set; }

        public int InactiveCourses { get; set; }

        public List<RecentStudentDto> RecentStudents { get; set; } = new();
    }
}