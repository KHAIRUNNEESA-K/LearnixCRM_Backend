using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.AdminReports
{
    public class LeadSummaryDto
    {
        public string Month { get; set; } = string.Empty;
        public int TotalLeads { get; set; }
        public int Converted { get; set; }
        public int Pending { get; set; }
        public decimal Revenue { get; set; }
    }
}
