using LearnixCRM.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.Student
{
    public class StudentResponseDto
    {
        public int StudentId { get; set; }

        public int LeadId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public CourseType Course { get; set; }

        public DateTime JoinedDate { get; set; }
    }
}
