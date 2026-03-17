using Dapper;
using LearnixCRM.Application.DTOs.AdminReports;
using LearnixCRM.Application.DTOs.Dashboard;
using LearnixCRM.Application.Interfaces.Repositories;
using System.Data;

namespace LearnixCRM.Infrastructure.Repositories
{
    public class AdminDashboardRepository : IAdminDashboardRepository
    {
        private readonly IDbConnection _connection;

        public AdminDashboardRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<AdminDashboardDto> GetAdminDashboardAsync()
        {
            using var multi = await _connection.QueryMultipleAsync(
                "sp_Admin_GetDashboard",
                commandType: CommandType.StoredProcedure);

            var dashboard = new AdminDashboardDto
            {
                LeadOverview = await multi.ReadFirstOrDefaultAsync<LeadOverviewDto>(),
                SalesOverview = await multi.ReadFirstOrDefaultAsync<SalesOverviewDto>(),
                MonthlyRevenue = (await multi.ReadAsync<MonthlyRevenueDto>()).ToList(),
                StudentEnrollments = (await multi.ReadAsync<StudentEnrollmentDto>()).ToList(),
                SalesReport = (await multi.ReadAsync<SalesReportDto>()).ToList(),
                SalesPerformance = (await multi.ReadAsync<SalesPerformanceDto>()).ToList(),
                PerformanceGoal = await multi.ReadFirstOrDefaultAsync<PerformanceGoalDto>(),
                ActiveUsers = (await multi.ReadAsync<ActiveUserDto>()).ToList()
            };

            return dashboard;
        }
    }
}