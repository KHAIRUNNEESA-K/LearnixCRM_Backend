using LearnixCRM.Domain.Common;
using LearnixCRM.Domain.Enum;

namespace LearnixCRM.Domain.Entities
{
    public class Lead : AuditableEntity
    {
        public int LeadId { get; private set; }

        public string FullName { get; private set; } = string.Empty;

        public string Email { get; private set; } = string.Empty;

        public string? Phone { get; private set; }

        public CourseType CourseInterested { get; private set; }

        public LeadSource Source { get; private set; }

        public LeadStatus Status { get; private set; }

        public int AssignedToUserId { get; private set; }

        public string? Remark { get; private set; }

        public ICollection<FollowUp> FollowUps { get; private set; }
            = new List<FollowUp>();

        public ICollection<LeadHistory> Histories { get; private set; }
            = new List<LeadHistory>();

        public Student? Student { get; private set; }

        private Lead() { }

        public Lead(
            string fullName,
            string email,
            string? phone,
            CourseType course,
            LeadSource source,
            int assignedTo,
            int createdBy)
        {
            FullName = fullName;
            Email = email.ToLowerInvariant();
            Phone = phone;
            CourseInterested = course;
            Source = source;
            AssignedToUserId = assignedTo;

            Status = LeadStatus.New;

            SetCreatedBy(createdBy);

            AddHistory("Lead Created", createdBy);
        }


        public void UpdateLead(
            string fullName,
            string email,
            string? phone,
            CourseType course,
            LeadSource source,
            int userId)
        {
            FullName = fullName;
            Email = email.ToLowerInvariant();
            Phone = phone;
            CourseInterested = course;
            Source = source;

            SetUpdated(userId);

            AddHistory("Lead Updated", userId);
        }

        public void UpdateStatus(
            LeadStatus status,
            int userId)
        {
            Status = status;

            SetUpdated(userId);

            AddHistory($"Status changed to {status}", userId);
        }


        public void UpdateRemark(
            string remark,
            int userId)
        {
            Remark = remark;

            SetUpdated(userId);

            AddHistory("Remark Updated", userId);
        }


        public void AssignTo(
            int assignedToUserId,
            int performedBy)
        {
            AssignedToUserId = assignedToUserId;

            SetUpdated(performedBy);

            AddHistory($"Assigned to User {assignedToUserId}", performedBy);
        }


        public void Delete(int userId)
        {
            SetDeleted(userId);

            AddHistory("Lead Deleted", userId);
        }


        public void RestoreLead(int userId)
        {
            Restore();

            SetUpdated(userId);

            AddHistory("Lead Restored", userId);
        }


        private void AddHistory(
            string action,
            int userId)
        {
            Histories.Add(
                new LeadHistory(
                    this,
                    action,
                    userId
                )
            );
        }

    }
}

