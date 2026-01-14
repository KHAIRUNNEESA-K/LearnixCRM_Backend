using AutoMapper;
using BCrypt.Net;
using LearnixCRM.Application.DTOs;
using LearnixCRM.Application.Interfaces;

namespace LearnixCRM.Application.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public AuthService(
            IAuthRepository authRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper)
        {
            _authRepository = authRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }


        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _authRepository.GetUserForLoginAsync(request.Email);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            if (!string.Equals(user.Status, "Active", StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("User is not active");

            var passwordValid = BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash
            );

            if (!passwordValid)
                throw new UnauthorizedAccessException("Invalid email or password");

            var token = _jwtTokenGenerator.GenerateJwtToken(
                user.UserId,
                user.Email,
                user.FullName,
                user.UserRole
            );

            var response = _mapper.Map<LoginResponseDto>(user);
            response.Token = token;

            return response;
        }
    }

}
