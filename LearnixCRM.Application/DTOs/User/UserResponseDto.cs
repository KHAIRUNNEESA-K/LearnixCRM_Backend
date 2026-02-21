using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.User
{
    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public int Role { get; set; } = default!; 
        public int Status { get; set; } = default!;
        public string ProfileImageUrl { get; set; }
        public string? EmployeeCode { get; private set; }
        public DateTime? DateOfJoining { get; private set; }
        public string? ContactNumber { get; private set; }
    }
}
