using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.AdminReports
{
    public class AdmissionSummaryDto
    {
        public string Course { get; set; } = string.Empty;
        public int StudentCount { get; set; }
        public decimal Revenue { get; set; }
        public string Trend { get; set; } = string.Empty;
    }
}
