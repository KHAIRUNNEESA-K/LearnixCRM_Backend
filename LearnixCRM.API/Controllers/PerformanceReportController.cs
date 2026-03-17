using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.AdminReports;
using LearnixCRM.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/admin/reports")]
    [Authorize(Policy = "AdminOnly")]
    public class PerformanceReportsController : ControllerBase
    {
        private readonly IPerformanceReportService _reportService;

        public PerformanceReportsController(IPerformanceReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("performance-dashboard")]
        public async Task<IActionResult> GetPerformanceDashboard()
        {
            var result = await _reportService.GetPerformanceDashboardAsync();

            return Ok(ApiResponse<PerformanceDashboardDto>.SuccessResponse(
                result,
                "Performance dashboard fetched successfully"
            ));
        }

        [HttpGet("performance-summary")]
        public async Task<IActionResult> GetPerformanceSummary()
        {
            var result = await _reportService.GetPerformanceSummaryAsync();

            return Ok(ApiResponse<IEnumerable<PerformanceSummaryDto>>.SuccessResponse(
                result,
                "Performance summary fetched successfully"
            ));
        }
    }
}
