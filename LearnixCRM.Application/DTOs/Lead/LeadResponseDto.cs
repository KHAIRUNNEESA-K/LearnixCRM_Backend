using LearnixCRM.Domain.Enum;

namespace LearnixCRM.Application.DTOs.Lead
{
    public class LeadResponseDto
    {
        public int LeadId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        public LeadSource Source { get; set; }
        public LeadStatus Status { get; set; }
        public int AssignedToUserId { get; set; }
        public string? Remark { get; set; }
    }
}

