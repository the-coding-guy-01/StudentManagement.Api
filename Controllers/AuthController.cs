using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.DTOs.Auth;
using StudentManagement.Api.Services.Interfaces;

namespace StudentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;


        public AuthController(
            IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>>
            Login(LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var result =
                await _authService.LoginAsync(request);


            if (result == null)
            {
                return Unauthorized(
                    new
                    {
                        message =
                            "Invalid username or password."
                    });
            }


            return Ok(result);
        }
    }
}