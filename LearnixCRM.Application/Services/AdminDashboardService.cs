using LearnixCRM.Application.DTOs.AdminReports;
using LearnixCRM.Application.DTOs.Dashboard;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Application.Interfaces.Services;

namespace LearnixCRM.Application.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardRepository _repository;

        public AdminDashboardService(IAdminDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<AdminDashboardDto> GetAdminDashboardAsync()
        {
            var dashboard = await _repository.GetAdminDashboardAsync();

            if (dashboard == null)
                throw new KeyNotFoundException("Admin dashboard data not found.");

            return dashboard;
        }
    }
}