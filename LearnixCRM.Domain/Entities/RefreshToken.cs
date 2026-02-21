using LearnixCRM.Domain.Common;

namespace LearnixCRM.Domain.Entities
{
    public class RefreshToken : AuditableEntity
    {
        public int RefreshTokenId { get; protected set; }

        public int UserId { get; private set; }

        public string Token { get; private set; }

        public DateTime ExpiresAt { get; private set; }

        public bool IsRevoked { get; private set; }


        public User User { get; private set; }

        protected RefreshToken() { }
        private RefreshToken(int userId, string token, DateTime expiresAt)
        {
            UserId = userId;
            Token = token;
            ExpiresAt = expiresAt;
            IsRevoked = false;
        }


        public static RefreshToken Create(int userId, string token, int expiryDays)
        {
            var refreshToken = new RefreshToken(
                userId,
                token,
                DateTime.UtcNow.AddDays(expiryDays)
            );

            refreshToken.SetCreatedBy(0);
            return refreshToken;
        }


        public void Revoke(int revokedByUserId)
        {
            if (IsRevoked)
                return;

            IsRevoked = true;

            SetUpdated(revokedByUserId);
        }

        public bool IsExpired()
        {
            return DateTime.UtcNow >= ExpiresAt;
        }
    }
}
