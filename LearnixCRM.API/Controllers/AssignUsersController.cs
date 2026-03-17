using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.AssignUsers;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/admin/team-sales")]
    [Authorize(Policy = "AdminOnly")]
    public class AssignUsersController : ControllerBase
    {
        private readonly IAssignUsersService _assignUsersService;

        public AssignUsersController(IAssignUsersService assignUsersService)
        {
            _assignUsersService = assignUsersService;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignSalesToTeam([FromBody] AssignSalesManagerRequestDto dto)
        {
            var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _assignUsersService.AssignSalesToTeamAsync(dto, adminId);

            return Ok(ApiResponse<AssignSalesManagerResponseDto>.SuccessResponse(
                result,
                "Sales user assigned to team successfully"
            ));
        }

        [HttpGet("manager/{managerUserId}/sales")]
        public async Task<IActionResult> GetManagerWithSales(int managerUserId)
        {
            var result = await _assignUsersService.GetManagerWithSalesAsync(managerUserId);

            return Ok(ApiResponse<ManagerWithSalesResponseDto>.SuccessResponse(
                result,
                "Manager sales fetched successfully"
            ));
        }

        [HttpPut("change-team")]
        public async Task<IActionResult> ChangeSalesTeam(
    int salesUserId,
    int newTeamId)
        {
            var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _assignUsersService.ChangeSalesTeamAsync(
                salesUserId,
                newTeamId,
                adminId);

            return Ok(ApiResponse<AssignSalesManagerResponseDto>.SuccessResponse(
                result,
                "Sales moved to new team successfully"));
        }
        [HttpDelete("{assignmentId}")]
        public async Task<IActionResult> RemoveSalesFromTeam(int assignmentId)
        {
            var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _assignUsersService.RemoveSalesFromTeamAsync(assignmentId, adminId);

            return Ok(ApiResponse<object>.SuccessResponse(
                null,
                "Sales removed from team successfully"));
        }
    }
}