using Dapper;
using LearnixCRM.Application.DTOs.AdminReports;
using LearnixCRM.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Infrastructure.Repositories
{
    public class PerformanceReportRepository : IPerformanceReportRepository
    {
        private readonly IDbConnection _connection;

        public PerformanceReportRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<PerformanceDashboardDto> GetPerformanceDashboardAsync()
        {
            return await _connection.QueryFirstOrDefaultAsync<PerformanceDashboardDto>(
                "sp_GetPerformanceDashboard",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<PerformanceSummaryDto>> GetPerformanceSummaryAsync()
        {
            return await _connection.QueryAsync<PerformanceSummaryDto>(
                "sp_GetPerformanceSummary",
                commandType: CommandType.StoredProcedure);
        }
    }
}
