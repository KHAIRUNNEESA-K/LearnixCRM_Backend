using LearnixCRM.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Domain.Entities
{
    public class AssignUsers : AuditableEntity
    {
        public int AssignId { get; private set; }

        public int SalesUserId { get; private set; }
        public User SalesUser { get; private set; }

        public int ManagerUserId { get; private set; }
        public User ManagerUser { get; private set; }

        public bool IsActive { get; private set; }

        protected AssignUsers() { }

        public static AssignUsers Create(int salesUserId,int managerUserId,int createdBy)
        {
            var assignment = new AssignUsers
            {
                SalesUserId = salesUserId,
                ManagerUserId = managerUserId,
                IsActive = true
            };

            assignment.SetCreatedBy(createdBy);

            return assignment;
        }

        public void Deactivate(int updatedBy)
        {
            IsActive = false;
            SetUpdated(updatedBy);
        }
        public void Delete(int deletedBy)
        {
            if (IsDeleted)
                return;

            IsActive = false;
            SetDeleted(deletedBy);
        }

    }
}