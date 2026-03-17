using Dapper;
using LearnixCRM.Application.DTOs.AdminReports;
using LearnixCRM.Application.Interfaces.Repositories;
using System.Data;

public class LeadReportRepository : ILeadReportRepository
{
    private readonly IDbConnection _connection;

    public LeadReportRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<LeadDashboardDto> GetDashboardAsync()
    {
        return await _connection.QueryFirstAsync<LeadDashboardDto>(
            "sp_GetLeadDashboard",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<LeadSummaryDto>> GetLeadSummaryAsync()
    {
        return await _connection.QueryAsync<LeadSummaryDto>(
            "sp_GetLeadSummary",
            commandType: CommandType.StoredProcedure);
    }
}