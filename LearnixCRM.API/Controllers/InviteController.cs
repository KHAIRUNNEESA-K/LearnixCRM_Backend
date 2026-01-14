using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs;
using LearnixCRM.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/invites")]
    [AllowAnonymous]
    public class InviteController : ControllerBase
    {
        private readonly IInviteService _inviteService;

        public InviteController(IInviteService inviteService)
        {
            _inviteService = inviteService;
        }

        [HttpGet("validate")]
        public async Task<IActionResult> ValidateInvite([FromQuery] string token)
        {
            var invite = await _inviteService.ValidateInviteAsync(token);

            if (invite == null)
                throw new ArgumentException("Invalid or expired invitation link");

            var response = ApiResponse<object>.SuccessResponse(
                new
                {
                    invite.Email,
                    invite.ExpiryDate
                },
                "Invitation is valid"
            );

            return Ok(response);
        }

        [HttpPost("accept")]
        public async Task<IActionResult> AcceptInvite([FromBody] AcceptInviteRequestDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                throw new ArgumentException("Password and confirm password do not match");

            await _inviteService.AcceptInviteAsync(dto.Token, dto.Password);

            var response = ApiResponse<object>.SuccessResponse(
                new { },
                "Account activated successfully"
            );

            return Ok(response);
        }
    }
}
