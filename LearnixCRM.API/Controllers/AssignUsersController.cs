using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.AssignUsers;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/admin/sales-manager")]
    [Authorize(Policy = "AdminOnly")]
    public class AssignUsersController : ControllerBase
    {
        private readonly IAssignUsersService _salesManager;

        public AssignUsersController(IAssignUsersService salesManager)
        {
            _salesManager = salesManager;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignSalesToManager([FromBody] AssignSalesManagerRequestDto dto)
        {
            try
            {
                var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var result =
                    await _salesManager.AssignSalesToManagerAsync(dto, adminId);

                return Ok(ApiResponse<AssignSalesManagerResponseDto>.SuccessResponse(
                    result,
                    "Sales user assigned to manager successfully"
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("manager/{managerUserId}/sales")]
        public async Task<IActionResult> GetManagerWithSales(int managerUserId)
        {
            var result = await _salesManager.GetManagerWithSalesAsync(managerUserId);

            return Ok(ApiResponse<ManagerWithSalesResponseDto>.SuccessResponse(
                result,
                "Manager with sales fetched successfully"
            ));
        }

        [HttpGet("sales/{salesUserId}/manager")]
        public async Task<IActionResult> GetSalesWithManager(int salesUserId)
        {
            var result = await _salesManager.GetSalesWithManagerAsync(salesUserId);

            return Ok(ApiResponse<SalesWithManagerResponseDto>.SuccessResponse(
                result,
                "Sales with manager fetched successfully"
            ));
        }

        [HttpPut("reassign")]
        public async Task<IActionResult> ReassignSalesToManager([FromBody] AssignSalesManagerRequestDto dto)
        {
            var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var team = await _salesManager.ChangeSalesManagerAsync(
                dto.SalesUserId,
                dto.ManagerUserId,
                adminId);

            return Ok(ApiResponse<AssignSalesManagerResponseDto>
                .SuccessResponse(team, "Sales user reassigned successfully"));
        }


        [HttpDelete("assign/{assignId}")]
        public async Task<IActionResult> DeleteAssignment(int assignId)
        {
            var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _salesManager.DeleteAssignmentAsync(assignId, adminId);

            return Ok(ApiResponse<object>.SuccessResponse(null, "Assignment deleted successfully"));
        }
        [HttpPut("reassign-team")]
        public async Task<IActionResult> ReassignManagerTeam([FromBody] ReassignManagerDto dto)
        {
            var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var newTeam = await _salesManager.ReassignManagerTeamAsync(dto.oldManagerUserId,dto.newManagerUserId,adminId);

            return Ok(ApiResponse<ManagerWithSalesResponseDto>.SuccessResponse(
                newTeam,
                "Manager team reassigned successfully"));
        }


    }
}
