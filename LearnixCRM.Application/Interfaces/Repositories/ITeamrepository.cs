using LearnixCRM.Application.DTOs.Team;
using LearnixCRM.Domain.Entities;

namespace LearnixCRM.Application.Interfaces.Repositories
{
    public interface ITeamRepository
    {
        Task<List<TeamResponseDto>> GetAllTeamsAsync();
        Task<Team?> GetTeamByIdAsync(int teamId);

        Task AddAsync(Team team);

        Task UpdateAsync(Team team);

        Task DeleteAsync(int teamId, int adminId);
        Task<Team?> GetByTeamNameAsync(string teamName);
        Task<Team?> GetByManagerIdAsync(int managerId);
        Task<List<Team>> GetTeamsByManagerIdAsync(int managerUserId);
        Task<bool> ManagerHasActiveTeamAsync(int managerUserId);
    }
}