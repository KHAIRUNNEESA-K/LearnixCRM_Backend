using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Domain.Common
{
    public abstract class AuditableEntity
    {
        public int CreatedBy { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public int? UpdatedBy { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public int? DeletedBy { get; private set; }

        public DateTime? DeletedAt { get; private set; }
        
        public bool IsDeleted => DeletedAt.HasValue;

        protected AuditableEntity()
        {
            
        }

        public void SetCreatedBy(int userId)
        {
            CreatedAt = DateTime.UtcNow;
            CreatedBy = userId;
        }

        public void SetUpdated(int userId)
        {
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = userId;
        }


        public void SetDeleted(int userId)
        {
            DeletedAt = DateTime.UtcNow;
            DeletedBy = userId;
        }


        public void Restore()
        {
            DeletedAt = null;

        }
    }
}
