using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.AssignUsers
{
    public class ReassignManagerDto
    {
        public int oldManagerUserId { get; set; }
        public int newManagerUserId { get; set; }
    }
}
