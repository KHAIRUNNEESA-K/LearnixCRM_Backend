using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.SetPasswordToken;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsersAsync();

            return Ok(ApiResponse<IEnumerable<UserResponseDto>>
                .SuccessResponse(users, "All users retrieved"));
        }

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var user = await _adminService.GetUserByIdAsync(userId);

            return Ok(ApiResponse<UserResponseDto>
                .SuccessResponse(user, "User retrieved"));
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveUsers()
        {
            var users = await _adminService.GetActiveUsersAsync();

            return Ok(ApiResponse<IEnumerable<UserResponseDto>>
                .SuccessResponse(users, "Active users retrieved successfully"));
        }

        [HttpGet("inactive")]
        public async Task<IActionResult> GetInactiveUsers()
        {
            var users = await _adminService.GetInactiveUsersAsync();

            return Ok(ApiResponse<IEnumerable<UserResponseDto>>
                .SuccessResponse(users, "Inactive users retrieved successfully"));
        }

        [HttpGet("rejected")]
        public async Task<IActionResult> GetRejectedUsers()
        {
            var users = await _adminService.GetRejectedUsersAsync();

            return Ok(ApiResponse<IEnumerable<RegisterUserResponseDto>>
                .SuccessResponse(users, "Rejected users retrieved successfully"));
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingUsers()
        {
            var users = await _adminService.GetPendingUsersAsync();

            return Ok(ApiResponse<IEnumerable<RegisterUserResponseDto>>
                .SuccessResponse(users, "Pending users retrieved"));
        }

        [HttpGet("blocked")]
        public async Task<IActionResult> GetBlockedUsers()
        {
            var users = await _adminService.GetBlockedUsersAsync();

            return Ok(ApiResponse<IEnumerable<UserResponseDto>>
                .SuccessResponse(users, "Blocked users retrieved"));
        }

        [HttpGet("active-users/{userId:int}")]
        public async Task<IActionResult> GetActiveUserById(int userId)
        {
            var user = await _adminService.GetActiveUserByIdAsync(userId);

            return Ok(ApiResponse<UserResponseDto>
                .SuccessResponse(user, "Active user retrieved successfully"));
        }

        [HttpPatch("{userId:int}/approve")]
        public async Task<IActionResult> ApproveUser(int userId)
        {
            var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var approvedUser = await _adminService.ApproveUserAndSendTokenAsync(userId, adminId);

            return Ok(ApiResponse<RegisterUserResponseDto>
                .SuccessResponse(approvedUser, "User approved successfully"));
        }

        [HttpPatch("reject")]
        public async Task<IActionResult> RejectUser([FromBody] RejectRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RejectReason))
                return BadRequest("Reject reason is required.");

            var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var rejectUser = await _adminService.RejectUserAsync(dto.UserId, adminId, dto.RejectReason);

            return Ok(ApiResponse<object>
                .SuccessResponse(rejectUser, "User rejected successfully"));
        }

        [HttpPatch("{userId:int}/block")]
        public async Task<IActionResult> BlockUser(int userId)
        {
            var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _adminService.BlockUserAsync(userId, adminId);

            return Ok(ApiResponse<object>
                .SuccessResponse(new { }, "User blocked successfully"));
        }

        [HttpPatch("{userId:int}/unblock")]
        public async Task<IActionResult> UnblockUser(int userId)
        {
            var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _adminService.UnblockUserAsync(userId, adminId);

            return Ok(ApiResponse<object>
                .SuccessResponse(new { }, "User unblocked successfully"));
        }

        [HttpDelete("{userId:int}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _adminService.DeleteUserAsync(userId, adminId);

            return Ok(ApiResponse<object>
                .SuccessResponse(new { }, "User deleted successfully"));
        }

        [HttpGet("managers")]
        public async Task<IActionResult> GetActiveManagers()
        {
            var result = await _adminService.GetActiveManagersAsync();

            return Ok(ApiResponse<IEnumerable<UserResponseDto>>
                .SuccessResponse(result, "Active managers retrieved successfully"));
        }

        [HttpGet("sales-executives")]
        public async Task<IActionResult> GetActiveSalesExecutives()
        {
            var result = await _adminService.GetActiveSalesExecutivesAsync();
            return Ok(ApiResponse<IEnumerable<UserResponseDto>>
                .SuccessResponse(result, "Active Sales executives retrieved successfully"));
        }

        [HttpPost("resend-set-password")]
        public async Task<IActionResult> ResendSetPassword(
          [FromBody] ForgotPasswordRequestDto dto)
        {
            var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            await _adminService
                .ResendSetPasswordTokenAsync(dto.Email, adminId);

            return Ok(ApiResponse<string>.SuccessResponse(
                null,
                "New set password link has been sent successfully"
            ));
        }

        [HttpGet("approved-pending-password")]
        public async Task<IActionResult> GetApprovedUsersPendingPassword()
        {
            var result = await _adminService.GetApprovedUsersPendingPasswordAsync();

            return Ok(ApiResponse<IEnumerable<UserResponseDto>>
                .SuccessResponse(result, "Approved users pending password setup retrieved successfully"));
        }
    }
}
