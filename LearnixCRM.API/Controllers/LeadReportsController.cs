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
    public class LeadReportsController : ControllerBase
    {
        private readonly ILeadReportService _reportService;

        public LeadReportsController(ILeadReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _reportService.GetDashboardAsync();

            return Ok(ApiResponse<LeadDashboardDto>.SuccessResponse(
                result,
                "Lead dashboard fetched successfully"
            ));
        }

        [HttpGet("lead-summary")]
        public async Task<IActionResult> GetLeadSummary()
        {
            var result = await _reportService.GetLeadSummaryAsync();

            return Ok(ApiResponse<IEnumerable<LeadSummaryDto>>.SuccessResponse(
                result,
                "Lead summary fetched successfully"
            ));
        }
    }
}