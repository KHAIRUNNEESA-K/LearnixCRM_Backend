using AutoMapper;
using LearnixCRM.Application.DTOs.Team;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;

namespace LearnixCRM.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAssignUsersRepository _assignUsersRepository;
        private readonly IMapper _mapper;

        public TeamService(
            ITeamRepository teamRepository,
            IUserRepository userRepository,
            IMapper mapper,IAssignUsersRepository assignUsersRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _assignUsersRepository = assignUsersRepository;
        }

        public async Task<TeamResponseDto> CreateTeamAsync(CreateTeamRequestDto dto, int adminId)
        {
            if (string.IsNullOrWhiteSpace(dto.TeamName))
                throw new InvalidOperationException("Team name is required");

            var existingTeam = await _teamRepository.GetByTeamNameAsync(dto.TeamName);
            if (existingTeam != null)
                throw new InvalidOperationException("Team name already exists");

            var managerAlreadyAssigned = await _teamRepository.GetByManagerIdAsync(dto.ManagerUserId);
            if (managerAlreadyAssigned != null)
                throw new InvalidOperationException("This manager already manages another team");

            var manager = await _userRepository.GetByIdAsync(dto.ManagerUserId)
                ?? throw new KeyNotFoundException("Manager not found");

            if (manager.UserRole != UserRole.Manager)
                throw new InvalidOperationException("User is not a Manager");

            if (manager.Status != UserStatus.Active)
                throw new InvalidOperationException("Manager must be Active");

            var team = Team.Create(dto.TeamName, dto.ManagerUserId, adminId);

            await _teamRepository.AddAsync(team);

            var response = _mapper.Map<TeamResponseDto>(team);
            response.ManagerName = manager.FullName;

            return response;
        }
        public async Task<List<TeamResponseDto>> GetAllTeamsAsync()
        {
            var teams = await _teamRepository.GetAllTeamsAsync();

            if (teams == null || !teams.Any())
                throw new InvalidOperationException("No teams found");

            return teams;
        }
        public async Task<TeamResponseDto> GetTeamByIdAsync(int teamId)
        {
            var team = await _teamRepository.GetTeamByIdAsync(teamId)
                ?? throw new KeyNotFoundException("Team not found");

            var manager = await _userRepository.GetByIdAsync(team.ManagerUserId);

            var response = _mapper.Map<TeamResponseDto>(team);

            response.ManagerName = manager?.FullName;

            return response;
        }
        public async Task<TeamResponseDto> UpdateTeamAsync(UpdateTeamRequestDto dto, int adminId)
        {
            var team = await _teamRepository.GetTeamByIdAsync(dto.TeamId)
                ?? throw new KeyNotFoundException("Team not found");

            if (string.IsNullOrWhiteSpace(dto.TeamName))
                throw new InvalidOperationException("Team name cannot be empty");

            var existingTeam = await _teamRepository.GetByTeamNameAsync(dto.TeamName);
            if (existingTeam != null && existingTeam.TeamId != dto.TeamId)
                throw new InvalidOperationException("Team name already exists");

            var manager = await _userRepository.GetByIdAsync(dto.ManagerUserId)
                ?? throw new KeyNotFoundException("Manager not found");

            if (manager.UserRole != UserRole.Manager)
                throw new InvalidOperationException("User is not a Manager");

            if (manager.Status != UserStatus.Active)
                throw new InvalidOperationException("Manager must be Active");

            var managerTeam = await _teamRepository.GetByManagerIdAsync(dto.ManagerUserId);
            if (managerTeam != null && managerTeam.TeamId != dto.TeamId)
                throw new InvalidOperationException("This manager already manages another team");

            team.ChangeName(dto.TeamName, adminId);
            team.ChangeManager(dto.ManagerUserId, adminId);

            await _teamRepository.UpdateAsync(team);

            var response = _mapper.Map<TeamResponseDto>(team);
            response.ManagerName = manager.FullName;

            return response;
        }

        public async Task DeleteTeamAsync(int teamId, int adminId)
        {
            var team = await _teamRepository.GetTeamByIdAsync(teamId)
                ?? throw new KeyNotFoundException("Team not found");

            if (!team.IsActive)
                throw new InvalidOperationException("Team already deleted");

            var salesMembers = await _assignUsersRepository.GetActiveByTeamIdAsync(teamId);

            if (salesMembers != null && salesMembers.Any())
                throw new InvalidOperationException(
                    "Cannot delete team because it has sales members. Reassign them to another team first."
                );

            await _teamRepository.DeleteAsync(teamId, adminId);
        }
        public async Task<TeamWithMembersResponseDto> GetTeamWithMembersAsync(int teamId)
        {
            var team = await _teamRepository.GetTeamByIdAsync(teamId)
                ?? throw new KeyNotFoundException("Team not found");

            if (!team.IsActive)
                throw new InvalidOperationException("Team is inactive");

            var manager = await _userRepository.GetByIdAsync(team.ManagerUserId)
                ?? throw new KeyNotFoundException("Manager not found");

            if (manager.UserRole != UserRole.Manager)
                throw new InvalidOperationException("Invalid manager role");

            var assignments = await _assignUsersRepository.GetSalesByTeamIdAsync(teamId);

            var members = _mapper.Map<List<SalesUserDto>>(assignments);

            var response = _mapper.Map<TeamWithMembersResponseDto>(team);

            response.TeamName=team.TeamName;
            response.ManagerName = manager.FullName;
            response.SalesMembers = members;

            return response;
        }
    }
}