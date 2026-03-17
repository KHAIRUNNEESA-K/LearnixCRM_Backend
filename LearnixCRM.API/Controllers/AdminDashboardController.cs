using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.AdminReports;
using LearnixCRM.Application.DTOs.Dashboard;
using LearnixCRM.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/admin/dashboard")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _dashboardService;

        public AdminDashboardController(IAdminDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAdminDashboard()
        {
            var result = await _dashboardService.GetAdminDashboardAsync();

            return Ok(ApiResponse<AdminDashboardDto>.SuccessResponse(
                result,
                "Admin dashboard fetched successfully"
            ));
        }
    }
}