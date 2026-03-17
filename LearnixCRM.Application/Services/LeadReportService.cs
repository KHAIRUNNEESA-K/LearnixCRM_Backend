using LearnixCRM.Application.DTOs.AdminReports;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Application.Interfaces.Services;

namespace LearnixCRM.Application.Services
{
    public class LeadReportService : ILeadReportService
    {
        private readonly ILeadReportRepository _reportRepository;

        public LeadReportService(ILeadReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<LeadDashboardDto> GetDashboardAsync()
        {
            var dashboard = await _reportRepository.GetDashboardAsync();

            if (dashboard == null)
                throw new KeyNotFoundException("Report dashboard data not found.");

            return dashboard;
        }

        public async Task<IEnumerable<LeadSummaryDto>> GetLeadSummaryAsync()
        {
            var summary = await _reportRepository.GetLeadSummaryAsync();

            if (summary == null || !summary.Any())
                throw new KeyNotFoundException("Lead summary report not found.");

            return summary;
        }
    }
}