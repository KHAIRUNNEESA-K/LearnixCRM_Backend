using System;

namespace LearnixCRM.Application.DTOs.Blacklist
{
    public class BlacklistResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Reason { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}