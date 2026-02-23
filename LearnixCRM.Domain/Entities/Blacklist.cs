using LearnixCRM.Domain.Common;

namespace LearnixCRM.Domain.Entities
{
    public class Blacklist : AuditableEntity
    {
        public int BlacklistId { get; private set; }

        public string Email { get; private set; } = string.Empty;

        public string? Phone { get; private set; }

        public string Reason { get; private set; } = string.Empty;

        public DateTime BlacklistedOn { get; private set; }

        private Blacklist() { }

        public Blacklist(
            string email,
            string? phone,
            string reason,
            int createdBy)
        {
            Email = email.ToLowerInvariant();
            Phone = phone;
            Reason = reason;
            BlacklistedOn = DateTime.UtcNow;

            SetCreatedBy(createdBy);
        }
    }
}
