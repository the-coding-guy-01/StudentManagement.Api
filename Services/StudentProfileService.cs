using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.DTOs.StudentPortal;
using StudentManagement.Api.Models;
using StudentManagement.Api.Services.Interfaces;

namespace StudentManagement.Api.Services
{
    public class StudentProfileService
        : IStudentProfileService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public StudentProfileService(
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<StudentProfileDto?>
            GetProfileAsync(int studentId)
        {
            var student = await _context.Students
                .AsNoTracking()
                .Include(x => x.StudentCourses)
                    .ThenInclude(x => x.Course)
                        .ThenInclude(x => x!.Teacher)
                .FirstOrDefaultAsync(x => x.Id == studentId);

            if (student == null)
            {
                return null;
            }

            return MapToDto(student);
        }

        public async Task<StudentProfileDto?>
            UpdateProfileAsync(
                int studentId,
                UpdateStudentProfileDto dto)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(
                    x => x.Id == studentId);

            if (student == null)
            {
                return null;
            }

            student.Phone = dto.Phone.Trim();
            student.FirstName = dto.FirstName.Trim();
            student.LastName = dto.LastName.Trim();
            student.Email = dto.Email.Trim();

            await _context.SaveChangesAsync();

            var updatedStudent = await _context.Students
                .AsNoTracking()
                .Include(x => x.StudentCourses)
                    .ThenInclude(x => x.Course)
                        .ThenInclude(x => x!.Teacher)
                .FirstOrDefaultAsync(x => x.Id == studentId);

            if (updatedStudent == null)
            {
                return null;
            }

            return MapToDto(updatedStudent);
        }

        public async Task<StudentProfileDto?>
            UploadProfileImageAsync(
                int studentId,
                IFormFile file)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(
                    x => x.Id == studentId);

            if (student == null)
            {
                return null;
            }

            if (file.Length == 0)
            {
                throw new InvalidOperationException(
                    "The selected file is empty.");
            }

            if (file.Length > 5 * 1024 * 1024)
            {
                throw new InvalidOperationException(
                    "Profile picture cannot exceed 5 MB.");
            }

            var allowedTypes =
                new[]
                {
                    "image/jpeg",
                    "image/png",
                    "image/webp"
                };

            if (!allowedTypes.Contains(
                    file.ContentType.ToLowerInvariant()))
            {
                throw new InvalidOperationException(
                    "Only JPG, PNG and WEBP images are allowed.");
            }

            var extension =
                Path.GetExtension(file.FileName)
                    .ToLowerInvariant();

            var allowedExtensions =
                new[]
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".webp"
                };

            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException(
                    "Invalid image extension.");
            }

            var folder =
                Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "student-profiles");

            Directory.CreateDirectory(folder);

            // Remove old picture
            DeleteExistingImage(
                student.ProfileImagePath);

            var fileName =
                $"{student.Id}_{Guid.NewGuid():N}{extension}";

            var fullPath =
                Path.Combine(
                    folder,
                    fileName);

            await using (var stream =
                new FileStream(
                    fullPath,
                    FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            student.ProfileImagePath =
                $"/uploads/student-profiles/{fileName}";

            await _context.SaveChangesAsync();

            var updatedStudent = await _context.Students
                .AsNoTracking()
                .Include(x => x.StudentCourses)
                    .ThenInclude(x => x.Course)
                        .ThenInclude(x => x!.Teacher)
                .FirstOrDefaultAsync(x => x.Id == studentId);

            if (updatedStudent == null)
            {
                return null;
            }

            return MapToDto(updatedStudent);
        }

        public async Task<bool>
            RemoveProfileImageAsync(
                int studentId)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(
                    x => x.Id == studentId);

            if (student == null)
            {
                return false;
            }

            DeleteExistingImage(
                student.ProfileImagePath);

            student.ProfileImagePath = null;

            await _context.SaveChangesAsync();

            return true;
        }

        private void DeleteExistingImage(
            string? imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                return;
            }

            var relativePath =
                imagePath.TrimStart('/')
                    .Replace(
                        '/',
                        Path.DirectorySeparatorChar);

            var fullPath =
                Path.Combine(
                    _environment.WebRootPath,
                    relativePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        private static StudentProfileDto
            MapToDto(Student student)
        {
            return new StudentProfileDto
            {
                Id = student.Id,

                AdmissionNumber =
                    student.AdmissionNumber,

                FirstName =
                    student.FirstName,

                LastName =
                    student.LastName,

                FullName =
                    $"{student.FirstName} {student.LastName}".Trim(),

                Email =
                    student.Email,

                Phone =
                    student.Phone,

                DateOfBirth =
                    student.DateOfBirth,

                Gender =
                    student.Gender,

                EnrollmentDate =
                    student.EnrollmentDate,

                IsActive =
                    student.IsActive,

                Courses =
                    student.StudentCourses
                        .Where(x => x.Course != null)
                        .Select(x => new StudentProfileCourseDto
                        {
                            CourseId =
                                x.Course!.Id,

                            CourseCode =
                                x.Course.CourseCode,

                            CourseName =
                                x.Course.CourseName,

                            Description =
                                x.Course.Description,

                            Credits =
                                x.Course.Credits,

                            Duration =
                                x.Course.Duration,

                            TeacherName =
                                x.Course.Teacher == null
                                    ? null
                                    : $"{x.Course.Teacher.FirstName} " +
                                      $"{x.Course.Teacher.LastName}"
                        })
                        .ToList(),

                ProfileImageUrl =
                    student.ProfileImagePath
            };
        }
    }
}