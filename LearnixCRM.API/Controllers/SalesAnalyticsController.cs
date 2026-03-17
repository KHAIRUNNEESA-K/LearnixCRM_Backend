using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.AdminReports;
using LearnixCRM.Application.DTOs.SalesAnalytics;
using LearnixCRM.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/admin/reports")]
    [Authorize(Policy = "AdminOnly")]
    public class SalesAnalyticsController : ControllerBase
    {
        private readonly ISalesAnalyticsService _salesService;

        public SalesAnalyticsController(ISalesAnalyticsService salesService)
        {
            _salesService = salesService;
        }

        [HttpGet("sales-summary")]
        public async Task<IActionResult> GetSalesSummary()
        {
            var result = await _salesService.GetSalesSummaryAsync();

            return Ok(ApiResponse<SalesAnalyticsSummaryDto>.SuccessResponse(
                result,
                "Sales summary fetched successfully"
            ));
        }

        [HttpGet("monthly-sales")]
        public async Task<IActionResult> GetMonthlySales()
        {
            var result = await _salesService.GetMonthlySalesAsync();

            return Ok(ApiResponse<IEnumerable<MonthlySalesDto>>.SuccessResponse(
                result,
                "Monthly sales fetched successfully"
            ));
        }

        [HttpGet("manager-performance")]
        public async Task<IActionResult> GetManagerPerformance()
        {
            var result = await _salesService.GetManagerPerformanceAsync();

            return Ok(ApiResponse<IEnumerable<ManagerPerformanceDto>>.SuccessResponse(
                result,
                "Manager performance fetched successfully"
            ));
        }
    }
}