using LearnixCRM.Domain.Enum;

namespace LearnixCRM.Application.DTOs.Lead
{
    public class UpdateLeadRequestDto
    {
        public int LeadId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public CourseType CourseInterested { get; set; }
        public LeadSource Source { get; set; }
        public string? Remark { get; set; }
        public LeadStatus? Status { get; set; }
    }
}
