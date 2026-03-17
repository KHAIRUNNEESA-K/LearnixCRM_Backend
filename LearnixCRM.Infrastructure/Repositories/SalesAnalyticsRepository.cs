using Dapper;
using LearnixCRM.Application.DTOs.SalesAnalytics;
using LearnixCRM.Application.Interfaces.Repositories;
using System.Data;

public class SalesAnalyticsRepository : ISalesAnalyticsRepository
{
    private readonly IDbConnection _connection;

    public SalesAnalyticsRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<SalesAnalyticsSummaryDto> GetSalesSummaryAsync()
    {
        var result = await _connection.QueryFirstOrDefaultAsync<SalesAnalyticsSummaryDto>(
            "sp_Admin_GetSalesAnalyticsSummary",
            commandType: CommandType.StoredProcedure
        );

        return result;
    }

    public async Task<IEnumerable<MonthlySalesDto>> GetMonthlySalesAsync()
    {
        var result = await _connection.QueryAsync<MonthlySalesDto>(
            "sp_Admin_GetMonthlySales",
            commandType: CommandType.StoredProcedure
        );

        return result;
    }

    public async Task<IEnumerable<ManagerPerformanceDto>> GetManagerPerformanceAsync()
    {
        var result = await _connection.QueryAsync<ManagerPerformanceDto>(
            "sp_Admin_GetManagerTeamPerformance",
            commandType: CommandType.StoredProcedure
        );

        return result;
    }
}