using LearnixCRM.Application.DTOs.AssignUsers;
using LearnixCRM.Application.DTOs.Team;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Application.Interfaces.Services;

namespace LearnixCRM.Application.Services
{
    public class AssignedSalesService : IAssignedSalesService
    {
        private readonly IAssignedSalesRepository _repository;

        public AssignedSalesService(IAssignedSalesRepository repository)
        {
            _repository = repository;
        }

        public async Task<ManagerWithSalesResponseDto> GetAllAssignedSalesAsync(int managerUserId)
        {
            if (managerUserId <= 0)
                throw new ArgumentException("Invalid manager id");

            var salesUsers = (await _repository.GetSalesByManagerAsync(managerUserId)).ToList();

            if (!salesUsers.Any())
                throw new KeyNotFoundException("No sales users assigned to this manager");

            return new ManagerWithSalesResponseDto
            {
                ManagerUserId = managerUserId,
                SalesUsers = salesUsers
            };
        }

        public async Task<SalesUserDto> GetAssignedSalesByIdAsync(int managerUserId, int salesUserId)
        {
            if (managerUserId <= 0 || salesUserId <= 0)
                throw new ArgumentException("Invalid manager or sales user id");

            var salesUser = await _repository.GetSalesByManagerAndSalesIdAsync(managerUserId, salesUserId);

            if (salesUser == null)
                throw new UnauthorizedAccessException(
                    "You are not authorized to view this sales user"
                );

            return salesUser;
        }

        public async Task<List<ManagerTeamDto>> GetManagerTeamsAsync(int managerUserId)
        {
            if (managerUserId <= 0)
                throw new ArgumentException("Invalid manager id");

            var teams = (await _repository.GetTeamsByManagerAsync(managerUserId)).ToList();

            if (!teams.Any())
                throw new KeyNotFoundException("No teams assigned to this manager");

            return teams;
        }

        public async Task<ManagerTeamWithMembersDto> GetTeamMembersAsync(int managerUserId, int teamId)
        {
            if (managerUserId <= 0 || teamId <= 0)
                throw new ArgumentException("Invalid manager id or team id");

            var teams = (await _repository.GetTeamsByManagerAsync(managerUserId)).ToList();

            var team = teams.FirstOrDefault(t => t.TeamId == teamId);

            if (team == null)
                throw new UnauthorizedAccessException(
                    "You are not authorized to view this team"
                );

            var members = (await _repository.GetTeamMembersAsync(teamId, managerUserId)).ToList();

            if (!members.Any())
                throw new KeyNotFoundException("No members found in this team");

            return new ManagerTeamWithMembersDto
            {
                TeamId = team.TeamId,
                TeamName = team.TeamName,
                Members = members
            };
        }
    }
}