using LearnixCRM.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.User
{
    public class UserProfileDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? ContactNumber { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? EmployeeCode { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public UserRole UserRole { get; set; }
        public UserStatus Status { get; set; }
    }
}
