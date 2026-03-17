using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.Dashboard
{
    public class AdminDashboardDto
    {
        public LeadOverviewDto LeadOverview { get; set; }
        public SalesOverviewDto SalesOverview { get; set; }

        public List<MonthlyRevenueDto> MonthlyRevenue { get; set; }
        public List<StudentEnrollmentDto> StudentEnrollments { get; set; }

        public List<SalesReportDto> SalesReport { get; set; }
        public List<SalesPerformanceDto> SalesPerformance { get; set; }

        public PerformanceGoalDto PerformanceGoal { get; set; }

        public List<ActiveUserDto> ActiveUsers { get; set; }
    }
}
