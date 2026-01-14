using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs;
using LearnixCRM.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);

        var response = ApiResponse<LoginResponseDto>.SuccessResponse(
            result,
            "Login successful"
        );

        return Ok(response);
    }
}
