using LearnixCRM.Application.Interfaces;
using LearnixCRM.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthTokenService : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public AuthTokenService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateJwtToken( int userId, string email, string fullName,string role)

       {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Secret)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
 {
    new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
    new Claim(ClaimTypes.Email, email),
    new Claim(ClaimTypes.Name, fullName), 
    new Claim(ClaimTypes.Role, role),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
};


        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
