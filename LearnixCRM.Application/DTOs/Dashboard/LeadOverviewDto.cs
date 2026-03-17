using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.Dashboard
{
    public class LeadOverviewDto
    {
        public int NewLeads { get; set; }
        public int Contacted { get; set; }
        public int Qualified { get; set; }
        public int Converted { get; set; }
        public int Lost { get; set; }
    }
}
