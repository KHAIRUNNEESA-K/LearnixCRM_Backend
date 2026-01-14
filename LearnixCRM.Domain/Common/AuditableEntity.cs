using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string? CreatedBy { get; private set; }
        public string? UpdatedBy { get; private set; }

        public DateTime? DeletedAt { get; private set; }
        public string? DeletedBy { get; private set; }
        public bool IsDeleted => DeletedAt.HasValue;

        protected AuditableEntity()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public void SetCreatedBy(string user)
        {
            CreatedBy = user;
        }

        public void SetUpdated(string user)
        {
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = user;
        }

        public void SetDeleted(string user)
        {
            DeletedAt = DateTime.UtcNow;
            DeletedBy = user;
        }

        public void Restore()
        {
            DeletedAt = null;
            DeletedBy = null;
        }
    }
}
