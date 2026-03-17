using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.Lead
{
    public class AdminLeadViewDto
    {
        public string LeadName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Source { get; set; }
        public string Team { get; set; }
        public string SalesUser { get; set; }
        public string Status { get; set; }
    }
}
