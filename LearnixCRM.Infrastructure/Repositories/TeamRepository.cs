using Dapper;
using LearnixCRM.Application.DTOs.Team;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LearnixCRM.Infrastructure.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly LearnixDbContext _context;
        private readonly IDbConnection _db;

        public TeamRepository(
            LearnixDbContext context,
            IDbConnection db)
        {
            _context = context;
            _db = db;
        }

        public async Task<List<TeamResponseDto>> GetAllTeamsAsync()
        {
            var result = await _db.QueryAsync<TeamResponseDto>(
                "sp_GetAllTeams",
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        public async Task<Team?> GetTeamByIdAsync(int teamId)
        {
            return await _db.QueryFirstOrDefaultAsync<Team>(
                "sp_GetTeamById",
                new { TeamId = teamId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task AddAsync(Team team)
        {
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Team team)
        {
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int teamId, int adminId)
        {
            var team = await _context.Teams
                .FirstOrDefaultAsync(x => x.TeamId == teamId && x.DeletedAt == null);

            if (team != null)
            {
                team.Deactivate(adminId);
                _context.Teams.Update(team);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Team?> GetByTeamNameAsync(string teamName)
        {
            return await _context.Teams
                .FirstOrDefaultAsync(x => x.TeamName == teamName && x.DeletedAt == null);
        }

        public async Task<Team?> GetByManagerIdAsync(int managerId)
        {
            return await _context.Teams
                .FirstOrDefaultAsync(x => x.ManagerUserId == managerId && x.DeletedAt == null);
        }
        public async Task<List<Team>> GetTeamsByManagerIdAsync(int managerUserId)
        {
            var teams = await _context.Teams
                .Where(x => x.ManagerUserId == managerUserId && x.DeletedAt == null)
                .ToListAsync();

            return teams;
        }
        public async Task<bool> ManagerHasActiveTeamAsync(int managerUserId)
        {
            return await _context.Teams
                .AnyAsync(x =>
                    x.ManagerUserId == managerUserId &&
                    x.IsActive &&
                    x.DeletedAt == null);
        }
    }
}