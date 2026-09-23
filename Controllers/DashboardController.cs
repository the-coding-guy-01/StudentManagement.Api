using Microsoft.AspNetCore.Mvc;
using StudentManagement.Api.DTOs.Dashboard;
using StudentManagement.Api.Services.Interfaces;

namespace StudentManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(
            IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }


        [HttpGet]
        public async Task<ActionResult<DashboardDto>> GetDashboard()
        {
            try
            {
                var dashboard =
                    await _dashboardService.GetDashboardAsync();

                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "DASHBOARD ERROR:");

                Console.WriteLine(ex.ToString());


                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "Dashboard failed to load.",

                        error =
                            ex.Message
                    });
            }
        }
    }
}
