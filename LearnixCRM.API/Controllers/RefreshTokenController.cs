using Azure.Core;
using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class RefreshTokenController : ControllerBase
{
    private readonly IRefreshTokenService _refreshTokenService;

    public RefreshTokenController(IRefreshTokenService refreshTokenService)
    {
        _refreshTokenService = refreshTokenService;
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized("Refresh token not found");
        var result = await _refreshTokenService.RefreshAsync(refreshToken);

        SetRefreshTokenCookie(result.RefreshToken);

        result.RefreshToken = null;

        return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(
            result,
            "Token refreshed successfully"
        ));
    }

    private void SetRefreshTokenCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false, // true in production
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}
