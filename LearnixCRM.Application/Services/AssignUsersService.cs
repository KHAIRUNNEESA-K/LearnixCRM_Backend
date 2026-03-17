using LearnixCRM.Application.DTOs.AssignUsers;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;

namespace LearnixCRM.Application.Services
{
    public class AssignUsersService : IAssignUsersService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IAssignUsersRepository _assignmentRepository;

        public AssignUsersService(
            IUserRepository userRepository,
            ITeamRepository teamRepository,
            IAssignUsersRepository assignmentRepository)
        {
            _userRepository = userRepository;
            _teamRepository = teamRepository;
            _assignmentRepository = assignmentRepository;
        }

        // Assign Sales to Team
        public async Task<AssignSalesManagerResponseDto> AssignSalesToTeamAsync(
            AssignSalesManagerRequestDto dto,
            int adminId)
        {
            var salesUser = await _userRepository.GetByIdAsync(dto.SalesUserId)
                ?? throw new KeyNotFoundException("Sales user not found");

            if (salesUser.UserRole != UserRole.Sales)
                throw new InvalidOperationException("User is not Sales");

            if (salesUser.Status != UserStatus.Active)
                throw new InvalidOperationException("Sales user must be Active");

            var team = await _teamRepository.GetTeamByIdAsync(dto.TeamId)
                ?? throw new KeyNotFoundException("Team not found");

            if (!team.IsActive)
                throw new InvalidOperationException("Team is inactive");

            var manager = await _userRepository.GetByIdAsync(team.ManagerUserId)
                ?? throw new KeyNotFoundException("Manager not found");

            var existingAssignment =
                await _assignmentRepository.GetActiveBySalesUserIdAsync(dto.SalesUserId);

            if (existingAssignment != null)
            {
                if (existingAssignment.TeamId == dto.TeamId)
                    throw new InvalidOperationException("Sales already assigned to this team");

                existingAssignment.Deactivate(adminId);
                await _assignmentRepository.UpdateAsync(existingAssignment);
            }

            var assignment = AssignUsers.Create(
                dto.TeamId,
                dto.SalesUserId,
                adminId);

            await _assignmentRepository.AddAsync(assignment);

            return new AssignSalesManagerResponseDto
            {
                SalesUserId = salesUser.UserId,
                SalesUserName = salesUser.FullName,
                SalesUserEmail = salesUser.Email,
                ManagerUserId = manager.UserId,
                ManagerName = manager.FullName,
                ManagerEmail = manager.Email
            };
        }

        public async Task<ManagerWithSalesResponseDto> GetManagerWithSalesAsync(int managerUserId)
        {
            var manager = await _userRepository.GetByIdAsync(managerUserId)
                ?? throw new KeyNotFoundException("Manager not found");

            if (manager.UserRole != UserRole.Manager)
                throw new InvalidOperationException("User is not Manager");

            var teams = await _teamRepository.GetTeamsByManagerIdAsync(managerUserId);

            if (teams == null || !teams.Any())
                throw new InvalidOperationException("Manager has no teams");

            var salesUsers = new List<SalesUserDto>();

            foreach (var team in teams)
            {
                var teamSales =
                    await _assignmentRepository.GetSalesByTeamIdAsync(team.TeamId);

                if (teamSales != null && teamSales.Any())
                    salesUsers.AddRange(teamSales);
            }

            return new ManagerWithSalesResponseDto
            {
                ManagerUserId = manager.UserId,
                ManagerName = manager.FullName,
                SalesUsers = salesUsers
            };
        }

        public async Task RemoveSalesFromTeamAsync(int assignmentId, int adminId)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(assignmentId)
                ?? throw new KeyNotFoundException("Assignment not found");

            if (!assignment.IsActive)
                throw new InvalidOperationException("Assignment already inactive");

            assignment.Deactivate(adminId);

            await _assignmentRepository.UpdateAsync(assignment);
        }

        public async Task<AssignSalesManagerResponseDto> ChangeSalesTeamAsync(
     int salesUserId,
     int newTeamId,
     int adminId)
        {
            var salesUser = await _userRepository.GetByIdAsync(salesUserId)
                ?? throw new KeyNotFoundException("Sales user not found");

            var team = await _teamRepository.GetTeamByIdAsync(newTeamId)
                ?? throw new KeyNotFoundException("Team not found");

            if (!team.IsActive)
                throw new InvalidOperationException("Team is inactive");

            var manager = await _userRepository.GetByIdAsync(team.ManagerUserId)
                ?? throw new KeyNotFoundException("Manager not found");

            var assignment =
                await _assignmentRepository.GetActiveBySalesUserIdAsync(salesUserId)
                ?? throw new InvalidOperationException("Sales not assigned to any team");

            if (assignment.TeamId == newTeamId)
                throw new InvalidOperationException("Sales already in this team");

            assignment.Deactivate(adminId);
            await _assignmentRepository.UpdateAsync(assignment);

            var newAssignment = AssignUsers.Create(
                newTeamId,
                salesUserId,
                adminId);

            await _assignmentRepository.AddAsync(newAssignment);

            return new AssignSalesManagerResponseDto
            {
                SalesUserId = salesUser.UserId,
                SalesUserName = salesUser.FullName,
                SalesUserEmail = salesUser.Email,
                ManagerUserId = manager.UserId,
                ManagerName = manager.FullName,
                ManagerEmail = manager.Email
            };
        }
    }
}