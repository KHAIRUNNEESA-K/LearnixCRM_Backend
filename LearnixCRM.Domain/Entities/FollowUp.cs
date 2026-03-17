using LearnixCRM.Domain.Common;

namespace LearnixCRM.Domain.Entities
{
    public class FollowUp : AuditableEntity
    {
        public int FollowUpId { get; private set; }

        public int LeadId { get; private set; }

        public DateTime FollowUpDate { get; private set; }

        public string Remark { get; private set; } = string.Empty;

        public Lead Lead { get; private set; } = null!;

        private FollowUp() { }

        public FollowUp(int leadId, DateTime followUpDate, string remark, int createdBy)
        {
            LeadId = leadId;
            FollowUpDate = followUpDate;
            Remark = remark;

            SetCreatedBy(createdBy);
        }

        public void UpdateFollowUp(DateTime followUpDate, string remark, int userId)
        {
            FollowUpDate = followUpDate;
            Remark = remark;
            SetUpdated(userId);
        }

           public void UpdateRemark(string remark, int userId)
        {
            Remark = remark;
            SetUpdated(userId);
        }


    }
}
