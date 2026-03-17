using Dapper;
using LearnixCRM.Application.DTOs.Lead;
using LearnixCRM.Application.Interfaces.Repositories;
using System.Data;

public class AdminLeadRepository : IAdminLeadRepository
{
    private readonly IDbConnection _db;

    public AdminLeadRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<AdminLeadViewDto>> GetAllLeadsAsync()
    {
        return await _db.QueryAsync<AdminLeadViewDto>(
            "sp_Admin_GetAllLeads",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<AdminLeadViewDto>> GetLeadsByStatusAsync(int status)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Status", status);

        return await _db.QueryAsync<AdminLeadViewDto>(
            "sp_Admin_GetLeadsByStatus",
            parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<AdminLeadViewDto>> GetLeadsByTeamAsync(int teamId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TeamId", teamId);

        return await _db.QueryAsync<AdminLeadViewDto>(
            "sp_Admin_GetLeadsByTeam",
            parameters,
            commandType: CommandType.StoredProcedure);
    }
}