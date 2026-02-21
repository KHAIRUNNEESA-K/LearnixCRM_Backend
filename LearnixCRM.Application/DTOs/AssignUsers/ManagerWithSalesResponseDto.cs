using LearnixCRM.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.AssignUsers
{
    public class ManagerWithSalesResponseDto
    {
        public int ManagerUserId { get; set; }
        public string ManagerName { get; set; }
        public List<SalesUserDto> SalesUsers { get; set; } = new();
    }
}
