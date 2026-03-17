using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;

namespace LearnixCRM.Application.DTOs.Lead
{
    public class CreateLeadRequestDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public int CourseId { get; set; }
        public LeadSource Source { get; set; }
    }
}