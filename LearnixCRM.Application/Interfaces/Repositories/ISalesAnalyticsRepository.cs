using LearnixCRM.Application.DTOs.SalesAnalytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Repositories
{
    public interface ISalesAnalyticsRepository
    {
        Task<SalesAnalyticsSummaryDto> GetSalesSummaryAsync();
        Task<IEnumerable<MonthlySalesDto>> GetMonthlySalesAsync();
        Task<IEnumerable<ManagerPerformanceDto>> GetManagerPerformanceAsync();
    }
}
