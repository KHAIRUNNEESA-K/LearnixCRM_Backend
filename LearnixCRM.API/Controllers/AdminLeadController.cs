using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.Lead;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/admin/leads")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminLeadController : ControllerBase
    {
        private readonly IAdminLeadService _adminLeadService;

        public AdminLeadController(IAdminLeadService adminLeadService)
        {
            _adminLeadService = adminLeadService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLeads()
        {
            var leads = await _adminLeadService.GetAllLeadsAsync();

            return Ok(ApiResponse<IEnumerable<AdminLeadViewDto>>
                .SuccessResponse(leads, "All leads retrieved successfully"));
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetLeadsByStatus(LeadStatus status)
        {
            var leads = await _adminLeadService.GetLeadsByStatusAsync(status);

            return Ok(ApiResponse<IEnumerable<AdminLeadViewDto>>
                .SuccessResponse(leads, $"Leads with status {status} retrieved successfully"));
        }

        [HttpGet("team/{teamId:int}")]
        public async Task<IActionResult> GetLeadsByTeam(int teamId)
        {
            var leads = await _adminLeadService.GetLeadsByTeamAsync(teamId);

            return Ok(ApiResponse<IEnumerable<AdminLeadViewDto>>
                .SuccessResponse(leads, "Team leads retrieved successfully"));
        }
    }
}