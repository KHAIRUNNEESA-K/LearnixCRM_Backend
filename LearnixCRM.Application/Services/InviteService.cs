using LearnixCRM.Application.Interfaces;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Services
{
    public class InviteService : IInviteService
    {
        private readonly IInviteRepository _repository;

        public InviteService(IInviteRepository repository)
        {
            _repository = repository;
        }
        public async Task<UserInvite?> ValidateInviteAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var invite = await _repository.GetInviteByTokenAsync(token);

            if (invite == null)
                return null;

            if (invite.IsUsed || invite.ExpiryDate <= DateTime.UtcNow)
                return null;

            return invite;
        }
        public async Task AcceptInviteAsync(string token, string password)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token is required");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required");

            var invite = await _repository.GetInviteByTokenAsync(token)
                ?? throw new KeyNotFoundException("Invalid invite");

            if (invite.IsUsed)
                throw new InvalidOperationException("Invite already used");

            if (invite.ExpiryDate <= DateTime.UtcNow)
                throw new InvalidOperationException("Invite expired");

            var user = await _repository.GetUserByEmailAsync(invite.Email)
                ?? throw new KeyNotFoundException("User not found");

            if (user.Status == UserStatus.Active)
                throw new InvalidOperationException("User already activated");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            user.Activate(passwordHash, updatedBy: user.Email);
            invite.MarkUsed(user.Email);

            await _repository.UpdateUserAsync(user);
            await _repository.SaveInviteAsync(invite);
        }

    }
}
