using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.SalesAnalytics
{
    public class SalesAnalyticsSummaryDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalSales { get; set; }
        public decimal AvgConversionRate { get; set; }
        public int ActiveTeams { get; set; }
    }
}
