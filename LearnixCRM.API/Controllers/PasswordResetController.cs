using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.Interfaces;
using LearnixCRM.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/password-reset")]
    public class PasswordResetController : ControllerBase
    {
        private readonly IPasswordResetService _passwordResetService;

        public PasswordResetController(IPasswordResetService passwordResetService)
        {
            _passwordResetService = passwordResetService;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestReset([FromBody] PasswordResetRequestDto dto)
        {
            await _passwordResetService.RequestResetAsync(dto.Email, requestedBy: dto.Email);

            var response = ApiResponse<object>.SuccessResponse(
                null,
                "Password reset link sent to email"
            );

            return Ok(response);
        }

        [HttpGet("validate")]
        public async Task<IActionResult> ValidateToken([FromQuery] string token)
        {
            await _passwordResetService.ValidateTokenAsync(token); 

            var response = ApiResponse<object>.SuccessResponse(
                null,
                "Token is valid"
            );

            return Ok(response);
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto dto)
        {
            await _passwordResetService.ResetPasswordAsync(dto.Token, dto.Password); 

            var response = ApiResponse<object>.SuccessResponse(
                null,
                "Password reset successful"
            );

            return Ok(response);
        }
    }
}
