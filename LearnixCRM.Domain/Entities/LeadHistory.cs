using LearnixCRM.Domain.Common;
using LearnixCRM.Domain.Enum;

namespace LearnixCRM.Domain.Entities
{
    public class LeadHistory : AuditableEntity
    {
        public int HistoryId { get; private set; }

        public int LeadId { get; private set; }

        public string FullName { get; private set; } = string.Empty;

        public string Email { get; private set; } = string.Empty;

        public string? Phone { get; private set; }

        public CourseType CourseInterested { get; private set; }

        public LeadSource Source { get; private set; }

        public LeadStatus Status { get; private set; }

        public int AssignedToUserId { get; private set; }

        public string? Remark { get; private set; }

        public string Action { get; private set; } = string.Empty;

        public Lead Lead { get; private set; } = null!;

        private LeadHistory() { }

        public LeadHistory(Lead lead, string action, int createdBy)
        {
            LeadId = lead.LeadId;

            FullName = lead.FullName;
            Email = lead.Email;
            Phone = lead.Phone;
            CourseInterested = lead.CourseInterested;
            Source = lead.Source;
            Status = lead.Status;
            AssignedToUserId = lead.AssignedToUserId;
            Remark = lead.Remark;

            Action = action;

            SetCreatedBy(createdBy);
        }
    }
}