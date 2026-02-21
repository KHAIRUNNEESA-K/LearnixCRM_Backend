using AutoMapper;
using BCrypt.Net;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;
using System.Security.Cryptography;

namespace LearnixCRM.Application.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IMapper _mapper;

        public AuthService(
            IAuthRepository authRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IRefreshTokenRepository refreshTokenRepository,
            IMapper mapper)
        {
            _authRepository = authRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
            _mapper=mapper;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _authRepository.GetUserForLoginAsync(request.Email);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            if (user.Status != UserStatus.Active)
                throw new UnauthorizedAccessException("User is not active");

            var passwordValid = BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash
            );

            if (!passwordValid)
                throw new UnauthorizedAccessException("Invalid email or password");

            await _refreshTokenRepository.RevokeAllByUserIdAsync(user.UserId);


            var accessToken = _jwtTokenGenerator.GenerateJwtToken(
                    user.UserId,
                    user.Email,
                    user.FullName,
                    (int)user.UserRole
                     );

            var refreshTokenString = GenerateSecureToken();

            var refreshToken = RefreshToken.Create(
                user.UserId,
                refreshTokenString,
                7
            );

            await _refreshTokenRepository.SaveAsync(refreshToken);

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenString,
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
        public async Task LogoutAsync(string refreshToken, int userId)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new UnauthorizedAccessException("Invalid request");

            var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (token == null)
                throw new UnauthorizedAccessException("Invalid refresh token");

            token.Revoke(userId);

            await _refreshTokenRepository.UpdateAsync(token);
        }



    }
}
