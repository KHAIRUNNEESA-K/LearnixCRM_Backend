using LearnixCRM.Application.DTOs.AdminReports;
using LearnixCRM.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Services
{
    public class AdmissionReportService : IAdmissionReportService
    {
        private readonly IAdmissionReportRepository _repository;

        public AdmissionReportService(IAdmissionReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<AdmissionDashboardDto> GetAdmissionDashboardAsync()
        {
            var dashboard = await _repository.GetAdmissionDashboardAsync();

            if (dashboard == null)
                throw new KeyNotFoundException("Admission dashboard data not found.");

            return dashboard;
        }

        public async Task<IEnumerable<AdmissionSummaryDto>> GetAdmissionSummaryAsync()
        {
            var summary = await _repository.GetAdmissionSummaryAsync();

            if (summary == null || !summary.Any())
                throw new KeyNotFoundException("Admission summary not found.");

            return summary;
        }
    }
}
