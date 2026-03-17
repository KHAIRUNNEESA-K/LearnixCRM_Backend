using LearnixCRM.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.Dashboard
{
    public class RoleUserDto
    {
        public UserRole UserRole { get; set; }

        public int TotalUsers { get; set; }
    }
}
