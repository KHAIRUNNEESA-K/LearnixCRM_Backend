using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.SetPasswordToken;
using LearnixCRM.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class SetPasswordController : ControllerBase
    {
        private readonly ISetPasswordService _setPasswordService;

        public SetPasswordController(ISetPasswordService setPasswordService)
        {
            _setPasswordService = setPasswordService;
        }

        [HttpPost("set-password")]
        public async Task<IActionResult> SetPassword( [FromBody] SetPasswordRequestDto dto)
        {
            var result = await _setPasswordService.SetPasswordAsync(dto);

            return Ok(ApiResponse<SetPasswordResponseDto>.SuccessResponse(
                result,
                "Password has been set successfully"
            ));
        }

        

    }
}
