using LearnixCRM.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Domain.Entities
{
    public class PasswordReset : AuditableEntity
    {
        public int ResetId { get; private set; }
        public string Email { get; private set; }
        public string Token { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public bool IsUsed { get; private set; }

        protected PasswordReset() { }

        public PasswordReset(string email)
        {
            Email = email.ToLower();
            Token = Guid.NewGuid().ToString();
            ExpiryDate = DateTime.UtcNow.AddMinutes(30);
            IsUsed = false;
        }

        public void MarkUsed(string updatedBy)
        {
            IsUsed = true;
            SetUpdated(updatedBy);
        }

    }
}
