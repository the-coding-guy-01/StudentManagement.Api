using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.DTOs;
using StudentManagement.Api.Models;
using StudentManagement.Api.Services.Interfaces;

namespace StudentManagement.Api.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDbContext _context;

        public TeacherService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<TeacherDto>> GetAllAsync()
        {
            var teachers = await _context.Teachers
                .OrderBy(s => s.Id)
                .ToListAsync();

            return teachers
                .Select(MapToDto)
                .ToList();
        }


        public async Task<TeacherDto?> GetByIdAsync(int id)
        {
            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(s => s.Id == id);

            if (teacher == null)
            {
                return null;
            }

            return MapToDto(teacher);
        }

        public async Task<int> GetTotalTeachersAsync()
        {
            return await _context.Teachers.CountAsync();
        }


        public async Task<TeacherDto> CreateAsync(
            CreateTeacherDto dto)
        {

            var emailExists =
    await _context.Teachers
        .AnyAsync(s => s.Email == dto.Email);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    "A teacher with this email already exists.");
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var teacher = new Teacher
                {
                    // Temporary unique value.
                    // The final admission number is generated
                    // after SQL Server creates the Teacher Id.

                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    DateOfBirth = dto.DateOfBirth,
                    Gender = dto.Gender,
                    Phone = dto.Phone,
                    JoinDate = dto.JoinDate,
                    IsActive = dto.IsActive
                };

                _context.Teachers.Add(teacher);
                await _context.SaveChangesAsync();


                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return MapToDto(teacher);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<TeacherDto?> UpdateAsync(
            int id,
            UpdateTeacherDto dto)
        {
            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(s => s.Id == id);

            if (teacher == null)
            {
                return null;
            }

            teacher.FirstName = dto.FirstName;
            teacher.LastName = dto.LastName;
            teacher.Email = dto.Email;
            teacher.DateOfBirth = dto.DateOfBirth;
            teacher.Gender = dto.Gender;
            teacher.Phone = dto.Phone;
            teacher.JoinDate = dto.JoinDate;
            teacher.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return MapToDto(teacher);
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


        private static TeacherDto MapToDto(Teacher teacher)
        {
            return new TeacherDto
            {
                Id = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,

                FullName =
                    $"{teacher.FirstName} {teacher.LastName}".Trim(),

                Email = teacher.Email,
                DateOfBirth = teacher.DateOfBirth,
                Gender = teacher.Gender,
                Phone = teacher.Phone,
                JoinDate = teacher.JoinDate,
                IsActive = teacher.IsActive
            };
        }
    }
}