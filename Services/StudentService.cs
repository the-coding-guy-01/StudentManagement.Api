using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.DTOs;
using StudentManagement.Api.Models;
using StudentManagement.Api.Services.Interfaces;

namespace StudentManagement.Api.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<StudentDto>> GetAllAsync()
        {
            var students = await _context.Students
                .Include(s => s.Course)
                .AsNoTracking()
                .ToListAsync();

            return students
                .Select(MapToDto)
                .ToList();
        }


        public async Task<StudentDto?> GetByIdAsync(int id)
        {
            var student = await _context.Students
                .Include(s => s.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return null;
            }

            return MapToDto(student);
        }

        public async Task<int> GetTotalStudentsAsync()
        {
            return await _context.Students.CountAsync();
        }


        public async Task<StudentDto> CreateAsync(
            CreateStudentDto dto)
        {

            var emailExists =
    await _context.Students
        .AnyAsync(s => s.Email == dto.Email);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    "A student with this email already exists.");
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var student = new Student
                {
                    // Temporary unique value.
                    // The final admission number is generated
                    // after SQL Server creates the Student Id.
                    AdmissionNumber =
                        $"TMP-{Guid.NewGuid().ToString("N")[..16]}",

                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    DateOfBirth = dto.DateOfBirth,
                    Gender = dto.Gender,
                    Phone = dto.Phone,
                    EnrollmentDate = dto.EnrollmentDate,
                    IsActive = dto.IsActive
                };

                _context.Students.Add(student);

                await _context.SaveChangesAsync();


                // SQL Server has now generated student.Id.
                //
                // Id = 1  -> 0001
                // Id = 2  -> 0002
                // Id = 15 -> 0015

                student.AdmissionNumber =
                    student.Id.ToString("D4");

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return MapToDto(student);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<StudentDto?> UpdateAsync(
            int id,
            UpdateStudentDto dto)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return null;
            }

            student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;
            student.Email = dto.Email;
            student.DateOfBirth = dto.DateOfBirth;
            student.Gender = dto.Gender;
            student.Phone = dto.Phone;
            student.EnrollmentDate = dto.EnrollmentDate;
            student.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return MapToDto(student);
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return false;
            }

            _context.Students.Remove(student);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<StudentDto?> AssignCourseAsync(
            int studentId,
            int courseId)
        {
            var student = await _context.Students
                .Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                return null;
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
            {
                return null;
            }

            // Assign / move student to course
            student.CourseId = course.Id;
            student.Course = course;

            await _context.SaveChangesAsync();

            return MapToDto(student);
        }

        public async Task<bool> RemoveCourseAsync(
            int studentId)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                return false;
            }

            student.CourseId = null;

            await _context.SaveChangesAsync();

            return true;
        }


        private static StudentDto MapToDto(Student student)
        {
            return new StudentDto
            {
                Id = student.Id,
                AdmissionNumber = student.AdmissionNumber,
                FirstName = student.FirstName,
                LastName = student.LastName,

                FullName =
                    $"{student.FirstName} {student.LastName}".Trim(),

                Email = student.Email,
                DateOfBirth = student.DateOfBirth,
                Gender = student.Gender,
                Phone = student.Phone,
                EnrollmentDate = student.EnrollmentDate,
                IsActive = student.IsActive,
                CourseId = student.CourseId,
                CourseName = student.Course?.CourseName

            };
        }
    }
}