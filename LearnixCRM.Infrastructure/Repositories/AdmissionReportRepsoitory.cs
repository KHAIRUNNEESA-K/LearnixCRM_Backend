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
    public class AdmissionReportRepsoitory :IAdmissionReportRepository
    {
        private readonly IDbConnection _connection;

        public AdmissionReportRepsoitory(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<AdmissionDashboardDto> GetAdmissionDashboardAsync()
        {
            return await _connection.QueryFirstOrDefaultAsync<AdmissionDashboardDto>(
                "sp_GetAdmissionDashboard",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AdmissionSummaryDto>> GetAdmissionSummaryAsync()
        {
            return await _connection.QueryAsync<AdmissionSummaryDto>(
                "sp_GetAdmissionSummary",
                commandType: CommandType.StoredProcedure);
        }
    }
}
