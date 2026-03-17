using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;
using Newtonsoft.Json.Linq;
using SendGrid.Helpers.Mail;
using System.Security.Cryptography;

namespace LearnixCRM.Application.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public RefreshTokenService(
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponseDto> RefreshAsync(string refreshToken)
        {
            var existingToken =
                await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (existingToken == null)
                throw new UnauthorizedAccessException("Invalid refresh token");

            Console.WriteLine("Token UserId BEFORE user fetch: " + existingToken.UserId);

            if (existingToken.IsRevoked)
                throw new UnauthorizedAccessException("Refresh token revoked");

            if (existingToken.IsExpired())
                throw new UnauthorizedAccessException("Refresh token expired");

            var user = await _refreshTokenRepository.GetByIdForAuthAsync(existingToken.UserId);
            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            if (user.Status != UserStatus.Active)
                throw new UnauthorizedAccessException("User not active");

            Console.WriteLine("User is null? " + (user == null));
            Console.WriteLine("User Status: " + user?.Status);


            existingToken.Revoke(user.UserId);
            Console.WriteLine("Token UserId: " + existingToken.UserId);
            await _refreshTokenRepository.UpdateAsync(existingToken);

            var newAccessToken = _jwtTokenGenerator.GenerateJwtToken(
                user.UserId,
                user.Email,
                user.FullName,
                (int)user.UserRole
            );
            

            var newRefreshTokenString = GenerateSecureToken();

            var newRefreshToken = RefreshToken.Create(
                user.UserId,
                newRefreshTokenString,
                7
            );

            await _refreshTokenRepository.SaveAsync(newRefreshToken);
            return new LoginResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshTokenString,
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = (int)user.UserRole
            };
        }


        private string GenerateSecureToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
