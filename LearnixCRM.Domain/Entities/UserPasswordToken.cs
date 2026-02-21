using LearnixCRM.Domain.Common;
using LearnixCRM.Domain.Enum;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace LearnixCRM.Domain.Entities
{
    public class UserPasswordToken : AuditableEntity
    {
        public int UserPasswordTokenId { get; private set; }

        public int UserId { get; private set; }
        public User User { get; private set; } = default!;

        public string TokenHash { get; private set; } = default!;
        public PasswordTokenType TokenType { get; private set; }

        public DateTime Expiry { get; private set; }
        public bool IsUsed { get; private set; }

        public DateTime? UsedAt { get; private set; }

        [NotMapped]
        public string RawToken { get; private set; } = default!;

        protected UserPasswordToken() { } 

        public static UserPasswordToken Create(int userId,PasswordTokenType tokenType,int createdBy,int expiryMinutes = 30)
        {
            var rawToken = GenerateSecureToken();

            var token = new UserPasswordToken
            {
                UserId = userId,
                TokenHash = Hash(rawToken),
                RawToken = rawToken,
                TokenType = tokenType,
                Expiry = DateTime.UtcNow.AddMinutes(expiryMinutes),
                IsUsed = false
            };

            token.SetCreatedBy(createdBy);

            return token;
        }

        public void MarkUsed(int userId)
        {
            if (IsUsed)
                throw new InvalidOperationException("Token already used.");

            if (IsExpired())
                throw new InvalidOperationException("Token expired.");

            IsUsed = true;
            UsedAt = DateTime.UtcNow;
            SetUpdated(userId);
        }


        public bool IsExpired()
        {
            return Expiry <= DateTime.UtcNow;
        }

        private static string GenerateSecureToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return WebEncoders.Base64UrlEncode(bytes);
        }

        public static string Hash(string input)
        {
            input = input.Trim();

            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }

    }
}
