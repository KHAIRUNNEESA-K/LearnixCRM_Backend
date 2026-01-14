using LearnixCRM.Application.Interfaces;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;
using System;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IPasswordResetRepository _repository;
        private readonly IEmailService _emailService;

        public PasswordResetService(IPasswordResetRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public async Task RequestResetAsync(string email, string requestedBy)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required");

            var user = await _repository.GetUserByEmailAsync(email);
            if (user == null)
                throw new Exception("User not found");

            var resetToken = Guid.NewGuid().ToString("N");

            await _repository.CreatePasswordResetAsync(email, resetToken, requestedBy);

            var link = $"https://app.learnixcrm.com/reset-password?token={resetToken}";
            await _emailService.SendResetPasswordAsync(email, link);
        }


        public async Task<PasswordReset?> ValidateTokenAsync(string token)
        {
            var reset = await _repository.GetByTokenAsync(token);
            if (reset == null || reset.IsUsed || reset.ExpiryDate <= DateTime.UtcNow)
                return null;

            return reset;
        }

        public async Task ResetPasswordAsync(string token, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required");

            var reset = await _repository.GetByTokenAsync(token)
                ?? throw new KeyNotFoundException("Invalid or expired token");

            if (reset.IsUsed || reset.ExpiryDate <= DateTime.UtcNow)
                throw new InvalidOperationException("Token is invalid or expired");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            await _repository.ResetPasswordAsync(reset.Email, passwordHash, token);
        }


    }
}
