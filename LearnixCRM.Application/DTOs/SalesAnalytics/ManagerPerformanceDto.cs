using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.SalesAnalytics
{
    public class ManagerPerformanceDto
    {
        public string ManagerName { get; set; }
        public string TeamName { get; set; }
        public int TotalSales { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal RevenueGenerated { get; set; }
    }
}
