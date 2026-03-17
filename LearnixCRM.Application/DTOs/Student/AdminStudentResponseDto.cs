using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.Student
{
    public class AdminStudentResponseDto
    {
            public int StudentId { get; set; }
            public string FullName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string? Phone { get; set; }
            public string CourseName { get; set; } = string.Empty;
            public decimal Fee { get; set; }
            public DateTime JoinedDate { get; set; }
            public string SalesName { get; set; } = string.Empty;
            public string? ManagerName { get; set; }
        
    }
}
