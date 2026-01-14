using LearnixCRM.Domain.Common;
using LearnixCRM.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Domain.Entities
{
    public class User : AuditableEntity
    {
        public int UserId { get; protected set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string? PasswordHash { get; private set; }
        public UserRole UserRole { get; private set; }
        public UserStatus Status { get; private set; }

        protected User() { }

        public static User CreateInvitedUser(string email, string fullName, UserRole role, string createdBy)
        {
            var user = new User
            {
                Email = email.ToLower(),
                FullName = fullName, // set it here
                UserRole = role,
                Status = UserStatus.Pending
            };

            user.SetCreatedBy(createdBy);
            return user;
        }

        public void Activate(string passwordHash, string updatedBy)
        {
            PasswordHash = passwordHash;
            Status = UserStatus.Active;
            SetUpdated(updatedBy);
        }

        public void Deactivate(string updatedBy)
        {
            Status = UserStatus.Inactive;
            SetUpdated(updatedBy);
        }

        public void ChangeRole(UserRole role, string updatedBy)
        {
            UserRole = role;
            SetUpdated(updatedBy);
        }
        public void Delete(string deletedBy)
        {
            if (IsDeleted)
                return;

            Status = UserStatus.Inactive;
            SetDeleted(deletedBy);
        }
        public void UpdatePassword(string newPasswordHash, string updatedBy)
        {
            PasswordHash = newPasswordHash;
            SetUpdated(updatedBy);
        }

    }
}

