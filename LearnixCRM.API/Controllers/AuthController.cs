using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        if (!string.IsNullOrEmpty(result.RefreshToken))
        {
            SetRefreshTokenCookie(result.RefreshToken);
        }


        result.RefreshToken = null;

        var response = ApiResponse<LoginResponseDto>.SuccessResponse(
            result,
            "Login successful"
        );

        return Ok(response);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var userId = int.Parse(User.FindFirst("UserId").Value);

        await _authService.LogoutAsync(refreshToken, userId);

        Response.Cookies.Delete("refreshToken");

        return Ok(ApiResponse<string>.SuccessResponse(
            null,
            "Logout successful"
        ));
    }


    private void SetRefreshTokenCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}
