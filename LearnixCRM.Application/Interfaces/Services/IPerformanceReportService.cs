using LearnixCRM.Application.DTOs.AdminReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Services
{
    public interface IPerformanceReportService
    {
        Task<PerformanceDashboardDto> GetPerformanceDashboardAsync();
        Task<IEnumerable<PerformanceSummaryDto>> GetPerformanceSummaryAsync();
    }
}
