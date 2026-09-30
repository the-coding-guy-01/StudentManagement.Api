using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.AdmissionNumber)
                .IsUnique();

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<StudentCourse>()
                .HasKey(x => new
                {
                    x.StudentId,
                    x.CourseId
                });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(x => x.Student)
                .WithMany(x => x.StudentCourses)
                .HasForeignKey(x => x.StudentId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(x => x.Course)
                .WithMany(x => x.StudentCourses)
                .HasForeignKey(x => x.CourseId);
        }
    }
}