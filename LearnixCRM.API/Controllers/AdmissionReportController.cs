using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.AdminReports;
using LearnixCRM.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/admin/reports")]
    [Authorize(Policy = "AdminOnly")]

    public class AdmissionReportController : ControllerBase
    {
        private readonly IAdmissionReportService _reportService;

        public AdmissionReportController(IAdmissionReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("admission-dashboard")]
        public async Task<IActionResult> GetAdmissionDashboard()
        {
            var result = await _reportService.GetAdmissionDashboardAsync();

            return Ok(ApiResponse<AdmissionDashboardDto>.SuccessResponse(
                result,
                "Admission dashboard fetched successfully"
            ));
        }

        [HttpGet("admission-summary")]
        public async Task<IActionResult> GetAdmissionSummary()
        {
            var result = await _reportService.GetAdmissionSummaryAsync();

            return Ok(ApiResponse<IEnumerable<AdmissionSummaryDto>>.SuccessResponse(
                result,
                "Admission summary fetched successfully"
            ));
        }
    }
}
