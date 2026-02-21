using AutoMapper;
using LearnixCRM.Application.DTOs.SetPasswordToken;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;

namespace LearnixCRM.Application.Services
{
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly ISetPasswordRepository _tokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public ResetPasswordService(
            ISetPasswordRepository tokenRepository,
            IUserRepository userRepository,
            IEmailService emailService,
            IMapper mapper)
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email)
                ?? throw new KeyNotFoundException("Email does not exist");

            if (user.Status != UserStatus.Active)
                throw new InvalidOperationException("User account is not active");

            var updatedBy = user.UserId;

            await _tokenRepository.InvalidateExistingTokensAsync(
                user.UserId,
                PasswordTokenType.ResetPassword,
                updatedBy);

            var token = UserPasswordToken.Create(
                user.UserId,
                PasswordTokenType.ResetPassword,
                updatedBy);

            Console.WriteLine($"User Email: {user.Email}");
            Console.WriteLine($"Token Type: {token.TokenType}"); 
            Console.WriteLine($"Raw Token (Use This in API): {token.RawToken}");
            Console.WriteLine($"Hashed Token (Stored in DB): {UserPasswordToken.Hash(token.RawToken)}");
       

            await _tokenRepository.CreateAsync(token);

            await _emailService.SendResetPasswordEmailAsync(
                user.Email,
                $"https://yourfrontend.com/reset-password?token={token.RawToken}");
        }



        public async Task<SetPasswordResponseDto> ResetPasswordAsync(SetPasswordRequestDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                throw new ArgumentException("Passwords do not match");

            var token = await GetValidTokenAsync(
                dto.Token,
                PasswordTokenType.ResetPassword);

            var user = await _userRepository.GetByIdAsync(token.UserId)
                ?? throw new KeyNotFoundException("User not found");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var updatedBy = user.UserId;

            user.UpdatePassword(passwordHash, updatedBy);
            token.MarkUsed(updatedBy);


            await _userRepository.UpdateAsync(user);
            await _tokenRepository.UpdateAsync(token);

            return _mapper.Map<SetPasswordResponseDto>(user);
        }

        private async Task<UserPasswordToken> GetValidTokenAsync(string rawToken,PasswordTokenType expectedType)
        {
            var hash = UserPasswordToken.Hash(rawToken);

            var token = await _tokenRepository.GetValidTokenAsync(hash)
                ?? throw new InvalidOperationException("Invalid or expired token");

            Console.WriteLine($"New Raw Token: {token.RawToken}");

            if (token.TokenType != expectedType)
                throw new InvalidOperationException("Invalid token type");

            return token;
        }

    }
}
