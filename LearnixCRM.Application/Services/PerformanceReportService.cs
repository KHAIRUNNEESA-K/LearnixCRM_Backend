using LearnixCRM.Application.DTOs.AdminReports;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Services
{
    public class PerformanceReportService : IPerformanceReportService
    {
        private readonly IPerformanceReportRepository _repository;

        public PerformanceReportService(IPerformanceReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<PerformanceDashboardDto> GetPerformanceDashboardAsync()
        {
            var dashboard = await _repository.GetPerformanceDashboardAsync();

            if (dashboard == null)
                throw new KeyNotFoundException("Performance dashboard data not found.");

            return dashboard;
        }

        public async Task<IEnumerable<PerformanceSummaryDto>> GetPerformanceSummaryAsync()
        {
            var summary = await _repository.GetPerformanceSummaryAsync();

            if (summary == null || !summary.Any())
                throw new KeyNotFoundException("Performance summary not found.");

            return summary;
        }
    }
}
