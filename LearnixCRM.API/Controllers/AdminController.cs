using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs;
using LearnixCRM.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("invite")]
        public async Task<IActionResult> InviteUser([FromBody] CreateUserRequestDto dto)
        {
            var adminName = User.Identity?.Name ?? "Admin";
            var user = await _adminService.InviteUserAsync(dto, adminName);

            var response = ApiResponse<UserResponseDto>.SuccessResponse(user, "Invitation sent successfully");
            return Ok(response);
        }

        [HttpGet("invites/pending")]
        public async Task<IActionResult> GetPendingInvites()
        {
            var invites = await _adminService.GetPendingInvitesAsync();
            var response = ApiResponse<IEnumerable<UserInviteDto>>.SuccessResponse(invites, "Pending invites retrieved");
            return Ok(response);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingUsers()
        {
            var users = await _adminService.GetPendingUsersAsync();
            var response = ApiResponse<IEnumerable<UserResponseDto>>.SuccessResponse(users, "Pending users retrieved");
            return Ok(response);
        }

        [HttpPut("{userId}/activate")]
        public async Task<IActionResult> ActivateUser(int userId)
        {
            var adminName = User.Identity?.Name ?? "Admin";
            await _adminService.ActivateUserAsync(userId, adminName);

            var response = ApiResponse<object>.SuccessResponse(new { }, "User activated successfully");
            return Ok(response);
        }

        [HttpPut("{userId}/deactivate")]
        public async Task<IActionResult> DeactivateUser(int userId)
        {
            var adminName = User.Identity?.Name ?? "Admin";
            await _adminService.DeactivateUserAsync(userId, adminName);

            var response = ApiResponse<object>.SuccessResponse(new { }, "User deactivated successfully");
            return Ok(response);
        }

        [HttpPut("{userId}/role")]
        public async Task<IActionResult> ChangeUserRole(int userId, [FromBody] ChangeRoleRequestDto dto)
        {
            var adminName = User.Identity?.Name ?? "Admin";
            await _adminService.ChangeUserRoleAsync(userId, dto.Role, adminName);

            var response = ApiResponse<object>.SuccessResponse(new { }, "User role updated successfully");
            return Ok(response);
        }

        [HttpPost("{userId}/resend-invite")]
        public async Task<IActionResult> ResendInvite(int userId)
        {
            var adminName = User.Identity?.Name ?? "Admin";

            await _adminService.ResendInviteAsync(userId, adminName);

            var response = ApiResponse<object>.SuccessResponse(
                new { },
                "Invitation resent successfully"
            );

            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            var response = ApiResponse<IEnumerable<UserResponseDto>>.SuccessResponse(users, "All users retrieved");
            return Ok(response);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var adminName = User.Identity?.Name ?? "Admin";
            await _adminService.DeleteUserAsync(userId, adminName);

            var response = ApiResponse<object>.SuccessResponse(new { }, "User deleted successfully");
            return Ok(response);
        }
    }
}
