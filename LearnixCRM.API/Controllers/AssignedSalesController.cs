using LearnixCRM.Application.Common.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Application.DTOs.AssignUsers;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Application.DTOs.Team;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/manager")]
    [Authorize(Policy = "ManagerOnly")]
    public class ManagerAssignedSalesController : ControllerBase
    {
        private readonly IAssignedSalesService _service;

        public ManagerAssignedSalesController(IAssignedSalesService service)
        {
            _service = service;
        }

        [HttpGet("sales")]
        public async Task<IActionResult> GetAllAssignedSales()
        {
            int managerUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _service.GetAllAssignedSalesAsync(managerUserId);

            return Ok(ApiResponse<ManagerWithSalesResponseDto>.SuccessResponse(
                result,
                "All assigned sales users fetched successfully"
            ));
        }

        [HttpGet("sales/{salesUserId}")]
        public async Task<IActionResult> GetAssignedSalesById(int salesUserId)
        {
            int managerUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _service.GetAssignedSalesByIdAsync(managerUserId, salesUserId);

            return Ok(ApiResponse<SalesUserDto>.SuccessResponse(
                result,
                "Assigned sales user fetched successfully"
            ));
        }

        [HttpGet("teams")]
        public async Task<IActionResult> GetManagerTeams()
        {
            int managerUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _service.GetManagerTeamsAsync(managerUserId);

            return Ok(ApiResponse<List<ManagerTeamDto>>.SuccessResponse(
                result,
                "Manager teams fetched successfully"
            ));
        }

        [HttpGet("teams/{teamId}/members")]
        public async Task<IActionResult> GetTeamMembers(int teamId)
        {
            int managerUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _service.GetTeamMembersAsync(managerUserId, teamId);

            return Ok(ApiResponse<ManagerTeamWithMembersDto>.SuccessResponse(
                result,
                "Team members fetched successfully"
            ));
        }
    }
}