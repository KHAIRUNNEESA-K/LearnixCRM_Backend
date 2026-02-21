using LearnixCRM.Domain.Common;
using LearnixCRM.Domain.Enum;

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
        public string? RejectReason { get; private set; }
        public string? EmployeeCode { get; private set; }
        public DateTime? DateOfJoining { get; private set; }
        public string? ContactNumber { get; private set; }
        public string? ProfileImageUrl { get; private set; }
        public string? ProfileImagePublicId { get; private set; }

        public ICollection<UserPasswordToken> PasswordTokens { get; private set; }
            = new List<UserPasswordToken>();
        public ICollection<RefreshToken> RefreshTokens { get; private set; }
            = new List<RefreshToken>();


        protected User() { }

        public static User CreateAdmin(string email, string fullName, string password)
        {
            var user = new User
            {
                Email = email.ToLower(),
                FullName = fullName,
                UserRole = UserRole.Admin,
                Status = UserStatus.Active,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };


            return user;
        }

        public static User CreateSelfRegisteredUser(
            string email,
            string firstName,
            string lastName,
            string contactNumber,
            UserRole role)
        {
            if (role == UserRole.Admin)
                throw new InvalidOperationException("Admin cannot self-register.");

            var fullName = $"{firstName} {lastName}";

            var user = new User
            {
                Email = email.ToLower(),
                FullName = fullName,
                UserRole = role,
                ContactNumber=contactNumber,
                Status = UserStatus.Pending
            };

            return user;
        }
        public void SetCreatedBySelf()
        {
            SetCreatedBy(this.UserId);
        }


        public void UpdateProfile(
            string fullName,
            string? contactNumber,
            string? profileImageUrl,
            string? profileImagePublicId,
            int updatedBy)
        {
            if (Status == UserStatus.Blocked)
                throw new InvalidOperationException("Blocked users cannot update profile.");

            FullName = fullName;
            ContactNumber = contactNumber;
            ProfileImageUrl = profileImageUrl;
            ProfileImagePublicId = profileImagePublicId;

            SetUpdated(updatedBy);
        }


        public void AssignEmployeeCode(string employeeCode, int updatedBy)
        {
            if (UserRole != UserRole.Manager && UserRole != UserRole.Sales)
                throw new InvalidOperationException("Employee code only applies to Manager and Sales roles.");

            if (!string.IsNullOrEmpty(EmployeeCode))
                throw new InvalidOperationException("Employee code already assigned.");

            EmployeeCode = employeeCode;
            DateOfJoining = DateTime.UtcNow;

            SetUpdated(updatedBy);
        }

        public void MarkApproved(int updatedBy)
        {
            if (Status != UserStatus.Pending)
                throw new InvalidOperationException("Only pending users can be approved.");

            Status = UserStatus.Approved;
            SetUpdated(updatedBy);
        }

        public void Activate(string passwordHash, int updatedBy)
        {
            if (Status != UserStatus.Approved)
                throw new InvalidOperationException("User must be approved first.");

            PasswordHash = passwordHash;
            Status = UserStatus.Active;

            if (DateOfJoining == null)
                DateOfJoining = DateTime.UtcNow;

            SetUpdated(updatedBy);
        }


        public void ActivateWithoutPassword(int updatedBy)
        {
            if (Status != UserStatus.Pending)
                throw new InvalidOperationException("Only pending users can be activated.");

            Status = UserStatus.Active;
            SetUpdated(updatedBy);
        }

        public void Reject(int adminUserId, string reason)
        {
            if (Status != UserStatus.Pending)
                throw new InvalidOperationException("Only pending users can be rejected.");

            Status = UserStatus.Rejected;
            RejectReason = reason;

            SetUpdated(adminUserId);
        }

        public void Block(int updatedBy)
        {
            if (Status != UserStatus.Active)
                throw new InvalidOperationException("Only active users can be blocked.");

            Status = UserStatus.Blocked;
            SetUpdated(updatedBy);
        }

        public void Unblock(int updatedBy)
        {
            if (Status != UserStatus.Blocked)
                throw new InvalidOperationException("Only blocked users can be unblocked.");

            Status = UserStatus.Active;
            SetUpdated(updatedBy);
        }

        public void Deactivate(int updatedBy)
        {
            Status = UserStatus.Inactive;
            SetUpdated(updatedBy);
        }

        public void Delete(int deletedBy)
        {
            if (IsDeleted)
                return;

            Status = UserStatus.Inactive;
            SetDeleted(deletedBy);
        }

        public void UpdatePassword(string newPasswordHash, int updatedBy)
        {
            PasswordHash = newPasswordHash;
            SetUpdated(updatedBy);
        }

        public void ChangeRole(UserRole role, int updatedBy)
        {
            UserRole = role;
            SetUpdated(updatedBy);
        }

    }
}
