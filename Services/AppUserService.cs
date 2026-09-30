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