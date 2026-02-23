using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LearnixCRM.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var result = ApiResponse<string>.FailureResponse(
                    string.Join(", ", errors),
                    400
                );

                return BadRequest(result);
            }

            var userDto = await _registrationService.RegisterAsync(dto);

            var response = ApiResponse<RegisterUserResponseDto>.SuccessResponse(
                userDto,
                "Registration successful. Pending admin approval."
            );

            return Ok(response);
        }
    }
}
