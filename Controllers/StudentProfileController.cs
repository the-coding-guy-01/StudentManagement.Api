using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.DTOs.StudentPortal;
using StudentManagement.Api.Services.Interfaces;
using System.Security.Claims;

namespace StudentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/student/profile")]
    [Authorize(Roles = "Student")]
    public class StudentProfileController
        : ControllerBase
    {
        private readonly IStudentProfileService
            _studentProfileService;


        public StudentProfileController(
            IStudentProfileService studentProfileService)
        {
            _studentProfileService =
                studentProfileService;
        }


        [HttpGet]
        public async Task<
            ActionResult<StudentProfileDto>>
            GetProfile()
        {
            if (!TryGetStudentId(
                    out var studentId))
            {
                return Forbid();
            }


            var profile =
                await _studentProfileService
                    .GetProfileAsync(studentId);


            if (profile == null)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Student profile was not found."
                    });
            }


            MakeImageUrlAbsolute(profile);

            return Ok(profile);
        }


        [HttpPut]
        public async Task<
            ActionResult<StudentProfileDto>>
            UpdateProfile(
                UpdateStudentProfileDto dto)
        {
            if (!TryGetStudentId(
                    out var studentId))
            {
                return Forbid();
            }


            var profile =
                await _studentProfileService
                    .UpdateProfileAsync(
                        studentId,
                        dto);


            if (profile == null)
            {
                return NotFound();
            }


            MakeImageUrlAbsolute(profile);

            return Ok(profile);
        }


        [HttpPost("photo")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<
            ActionResult<StudentProfileDto>>
            UploadPhoto(
                IFormFile file)
        {
            if (!TryGetStudentId(
                    out var studentId))
            {
                return Forbid();
            }


            try
            {
                var profile =
                    await _studentProfileService
                        .UploadProfileImageAsync(
                            studentId,
                            file);


                if (profile == null)
                {
                    return NotFound();
                }


                MakeImageUrlAbsolute(profile);

                return Ok(profile);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(
                    new
                    {
                        message = ex.Message
                    });
            }
        }


        [HttpDelete("photo")]
        public async Task<IActionResult>
            RemovePhoto()
        {
            if (!TryGetStudentId(
                    out var studentId))
            {
                return Forbid();
            }


            var removed =
                await _studentProfileService
                    .RemoveProfileImageAsync(
                        studentId);


            if (!removed)
            {
                return NotFound();
            }


            return NoContent();
        }


        private bool TryGetStudentId(
            out int studentId)
        {
            var studentIdValue =
                User.FindFirstValue("StudentId");


            return int.TryParse(
                studentIdValue,
                out studentId);
        }


        private void MakeImageUrlAbsolute(
            StudentProfileDto profile)
        {
            if (string.IsNullOrWhiteSpace(
                    profile.ProfileImageUrl))
            {
                return;
            }


            if (profile.ProfileImageUrl
                .StartsWith("/"))
            {
                profile.ProfileImageUrl =
                    $"{Request.Scheme}://" +
                    $"{Request.Host}" +
                    profile.ProfileImageUrl;
            }
        }
    }
}