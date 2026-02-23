using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.Lead;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Application.Services;
using LearnixCRM.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/sales/leads")]
    [Authorize(Policy = "SalesOnly")]
    public class SalesLeadController : ControllerBase
    {
        private readonly ISalesLeadService _service;

        public SalesLeadController(ISalesLeadService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLeads()
        {
            int salesUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var leads = await _service.GetAllLeadsAsync();

            return Ok(ApiResponse<IEnumerable<LeadResponseDto>>.SuccessResponse(
                leads,
                "All leads fetched successfully"
            ));
        }

        [HttpGet("{leadId}")]
        public async Task<IActionResult> GetLeadById(int leadId)
        {
            int salesUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var lead = await _service.GetLeadByIdAsync(leadId);

            return Ok(ApiResponse<LeadResponseDto>.SuccessResponse(
                lead,
                "Lead fetched successfully"
            ));
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetLeadsByStatus(LeadStatus status)
        {
            int salesUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var leads = await _service.GetLeadsByStatusAsync(status);

            return Ok(ApiResponse<IEnumerable<LeadResponseDto>>.SuccessResponse(
                leads,
                $"Leads with status {status} fetched successfully"
            ));
        }

        [HttpPost]
        public async Task<IActionResult> CreateLead([FromBody] CreateLeadRequestDto dto)
        {
            int salesUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var lead = await _service.AddLeadAsync(dto, salesUserId);

            return Ok(ApiResponse<LeadResponseDto>.SuccessResponse(
                lead,
                "Lead created successfully"
            ));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLead([FromBody] UpdateLeadRequestDto dto)
        {
            int salesUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var lead = await _service.UpdateLeadAsync(dto, salesUserId);

            return Ok(ApiResponse<LeadResponseDto>.SuccessResponse(
                lead,
                "Lead updated successfully"
            ));
        }

        [HttpDelete("{leadId}")]
        public async Task<IActionResult> DeleteLead(int leadId)
        {
            int salesUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _service.DeleteLeadAsync(leadId, salesUserId);

            return Ok(ApiResponse<string>.SuccessResponse(
                null,
                "Lead deleted successfully"
            ));
        }
    }
}
