using AutoMapper;
using LearnixCRM.Application.DTOs.SetPasswordToken;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;

namespace LearnixCRM.Application.Services
{
    public class SetPasswordService : ISetPasswordService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISetPasswordRepository _tokenRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public SetPasswordService(IUserRepository userRepository,ISetPasswordRepository tokenRepository, IMapper mapper,IEmailService emailService)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _mapper = mapper;
            _emailService=emailService;
        }

        public async Task<SetPasswordResponseDto> SetPasswordAsync(SetPasswordRequestDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                throw new ArgumentException("Passwords do not match");

            var token = await GetValidTokenAsync(
                dto.Token,
                PasswordTokenType.SetPassword);

            var user = await _userRepository.GetByIdAsync(token.UserId)
                ?? throw new KeyNotFoundException("User not found");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var updatedBy = user.UserId;
            if (user.PasswordHash == null)
            {
                user.Activate(passwordHash, updatedBy);
            }
            else
            {
                user.UpdatePassword(passwordHash, updatedBy);
            }


            token.MarkUsed(updatedBy);


            await _userRepository.UpdateAsync(user);
            await _tokenRepository.UpdateAsync(token);

            return _mapper.Map<SetPasswordResponseDto>(user);
        }

        private async Task<UserPasswordToken> GetValidTokenAsync(string rawToken, PasswordTokenType expectedType)
        {
            rawToken = rawToken.Trim();
            var hash = UserPasswordToken.Hash(rawToken);

            Console.WriteLine($"Incoming Raw Token: {rawToken}");
            Console.WriteLine($"Generated Hash: {hash}");

            var token = await _tokenRepository.GetValidTokenAsync(hash);

            if (token == null)
            {
                Console.WriteLine("Token NOT found in DB");
                throw new InvalidOperationException("Invalid or expired token");
            }

            Console.WriteLine($"DB TokenHash: {token.TokenHash}");
            Console.WriteLine($"DB Expiry: {token.Expiry}");
            Console.WriteLine($"DB IsUsed: {token.IsUsed}");

            if (token.TokenType != expectedType)
                throw new InvalidOperationException("Invalid token type");

            return token;
        }

        

    }
}
