using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.DTOs.AppUsers;
using StudentManagement.Api.Models;
using StudentManagement.Api.Services.Interfaces;

namespace StudentManagement.Api.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<AppUser> _passwordHasher;

        public AppUserService(
            ApplicationDbContext context,
            IPasswordHasher<AppUser> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> CreateUserAsync(CreateUserDto request)
        {
            var role = request.Role.Trim();
            if (role != "Student" && role != "Teacher") return false;
            if (role == "Student" && request.EnrollmentDate == null) return false;
            if (role == "Teacher" && request.JoinDate == null) return false;
            if (await _context.AppUsers.AnyAsync(x => x.Username == request.Email) ||
                await _context.Students.AnyAsync(x => x.Email == request.Email) ||
                await _context.Teachers.AnyAsync(x => x.Email == request.Email)) return false;

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = new AppUser { Role = role, IsActive = true };
                if (role == "Student")
                {
                    var student = new Student
                    {
                        AdmissionNumber = $"TMP-{Guid.NewGuid():N}"[..20],
                        FirstName = request.FirstName, LastName = request.LastName,
                        Email = request.Email, DateOfBirth = request.DateOfBirth,
                        Gender = request.Gender, Phone = request.Phone,
                        EnrollmentDate = request.EnrollmentDate!.Value, IsActive = true
                    };
                    _context.Students.Add(student);
                    await _context.SaveChangesAsync();
                    student.AdmissionNumber = student.Id.ToString("D4");
                    user.Username = student.AdmissionNumber;
                    user.StudentId = student.Id;
                }
                else
                {
                    var teacher = new Teacher
                    {
                        FirstName = request.FirstName, LastName = request.LastName,
                        Email = request.Email, DateOfBirth = request.DateOfBirth,
                        Gender = request.Gender, Phone = request.Phone,
                        JoinDate = request.JoinDate!.Value, IsActive = true
                    };
                    _context.Teachers.Add(teacher);
                    await _context.SaveChangesAsync();
                    user.Username = request.Email;
                    user.TeacherId = teacher.Id;
                }

                user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
                _context.AppUsers.Add(user);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<bool> RegisterStudentAsync(
            RegisterStudentUserDto request)
        {
            // Find the existing student
            var student = await _context.Students
                .FirstOrDefaultAsync(x =>
                    x.Id == request.StudentId);

            if (student == null)
            {
                return false;
            }


            // Check whether this student already has
            // an AppUser account
            var alreadyRegistered =
                await _context.AppUsers
                    .AnyAsync(x =>
                        x.StudentId == student.Id);

            if (alreadyRegistered)
            {
                return false;
            }


            // Also make sure the admission number
            // isn't already being used as a username
            var usernameExists =
                await _context.AppUsers
                    .AnyAsync(x =>
                        x.Username == student.AdmissionNumber);

            if (usernameExists)
            {
                return false;
            }


            var appUser = new AppUser
            {
                Username = student.AdmissionNumber,
                Role = "Student",
                StudentId = student.Id,
                TeacherId = null,
                IsActive = true
            };


            // Hash the password before saving
            appUser.PasswordHash =
                _passwordHasher.HashPassword(
                    appUser,
                    request.Password);


            _context.AppUsers.Add(appUser);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<int>> GetRegisteredStudentIdsAsync()
        {
            return await _context.AppUsers
                .Where(x =>
                    x.Role == "Student" &&
                    x.StudentId.HasValue)
                .Select(x => x.StudentId!.Value)
                .ToListAsync();
        }
    }


}
