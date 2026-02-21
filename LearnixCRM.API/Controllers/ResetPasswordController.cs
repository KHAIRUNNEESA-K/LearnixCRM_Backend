using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.SetPasswordToken;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Sprache;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class ResetPasswordController : ControllerBase
    {
        private readonly IResetPasswordService _service;

        public ResetPasswordController(IResetPasswordService service)
        {
            _service = service;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto dto)
        {
            await _service.ForgotPasswordAsync(dto.Email);

            return Ok(ApiResponse<object>.SuccessResponse(
                new { },
                " reset link has been sent"
            ));
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] SetPasswordRequestDto dto)
        {
            var result = await _service.ResetPasswordAsync(dto);

            return Ok(ApiResponse<SetPasswordResponseDto>.SuccessResponse(
                result,
                "Password has been set successfully"
            ));
        }
    }

}
