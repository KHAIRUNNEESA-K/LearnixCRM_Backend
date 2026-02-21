using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.AssignUsers
{
    public class AssignSalesManagerResponseDto
    {
        public int SalesUserId { get; set; }
        public string SalesUserName { get; set; }
        public string SalesUserEmail { get; set; }

        public int ManagerUserId { get; set; }
        public string ManagerName { get; set; }
        public string ManagerEmail { get; set; }
    }
}
