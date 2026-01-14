using LearnixCRM.Domain.Common;
using System;

namespace LearnixCRM.Domain.Entities
{
    public class UserInvite : AuditableEntity
    {
        public int InviteId { get; private set; }
        public string Email { get; private set; }
        public string Token { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public bool IsUsed { get; private set; }

        protected UserInvite() { }
        public UserInvite(string email, string createdBy)
        {
            Email = email.ToLower();
            Token = Guid.NewGuid().ToString();
            ExpiryDate = DateTime.UtcNow.AddDays(2);
            IsUsed = false;

            SetCreatedBy(createdBy); 
        }

        public void ExtendExpiry(string updatedBy, int days = 2)
        {
            ExpiryDate = DateTime.UtcNow.AddDays(days);
            SetUpdated(updatedBy); 
        }

        public void MarkUsed(string updatedBy)
        {
            IsUsed = true;
            SetUpdated(updatedBy);
        }
    }
}
