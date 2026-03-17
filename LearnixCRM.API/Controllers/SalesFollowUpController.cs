using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.FollowUp;
using LearnixCRM.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/sales/followups")]
    [Authorize(Policy = "SalesOnly")]
    public class SalesFollowUpController : ControllerBase
    {
        private readonly ISalesFollowUpService _service;

        public SalesFollowUpController(ISalesFollowUpService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFollowUps()
        {
            int salesUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var followUps = await _service.GetAllFollowUpsAsync(salesUserId);

            return Ok(ApiResponse<IEnumerable<FollowUpResponseDto>>.SuccessResponse(
                followUps,
                "All follow-ups fetched successfully"
            ));
        }

        [HttpGet("{followUpId}")]
        public async Task<IActionResult> GetFollowUpById(int followUpId)
        {
            int salesUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var followUp = await _service.GetFollowUpByIdAsync(followUpId, salesUserId);

            return Ok(ApiResponse<FollowUpResponseDto>.SuccessResponse(
                followUp,
                "Follow-up fetched successfully"
            ));
        }

        [HttpPost]
        public async Task<IActionResult> CreateFollowUp([FromBody] CreateFollowUpRequestDto dto)
        {
            int salesUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var followUp = await _service.AddFollowUpAsync(dto, salesUserId);

            return Ok(ApiResponse<FollowUpResponseDto>.SuccessResponse(
                followUp,
                "Follow-up created successfully"
            ));
        }

        [HttpPut("{followUpId}")]
        public async Task<IActionResult> UpdateFollowUp(int followUpId, [FromBody] UpdateFollowUpRequestDto dto)
        {
            int salesUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var followUp = await _service.UpdateFollowUpAsync(followUpId, dto, salesUserId);

            return Ok(ApiResponse<FollowUpResponseDto>.SuccessResponse(
                followUp,
                "Follow-up updated successfully"
            ));
        }

        [HttpDelete("{followUpId}")]
        public async Task<IActionResult> DeleteFollowUp(int followUpId)
        {
            int salesUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _service.DeleteFollowUpAsync(followUpId, salesUserId);

            return Ok(ApiResponse<string>.SuccessResponse(
                null,
                "Follow-up deleted successfully"
            ));
        }
    }
}
