using System;

namespace LearnixCRM.Application.DTOs.FollowUp
{
    public class UpdateFollowUpRequestDto
    {
        public DateTime FollowUpDate { get; set; }
        public string Remark { get; set; } = string.Empty;
    }
}