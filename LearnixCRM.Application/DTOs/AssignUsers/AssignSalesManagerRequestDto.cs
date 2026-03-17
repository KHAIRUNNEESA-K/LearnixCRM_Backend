using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.AssignUsers
{
    public class AssignSalesManagerRequestDto
   {
        public int TeamId { get; set; }
        public int SalesUserId { get; set; }
    }
}
