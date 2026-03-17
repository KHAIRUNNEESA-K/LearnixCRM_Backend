using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.AdminReports
{
    public class AdmissionDashboardDto
    {
        public int TotalAdmissions { get; set; }
        public decimal GrowthRate { get; set; }
        public decimal Efficiency { get; set; }
    }
}
