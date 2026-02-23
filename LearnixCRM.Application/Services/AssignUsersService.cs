using LearnixCRM.Application.DTOs.AssignUsers;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Services
{
    public class AssignUsersService : IAssignUsersService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAssignUsersRepository _assignmentRepository;

        public AssignUsersService(
            IUserRepository userRepository,
            IAssignUsersRepository assignmentRepository)
        {
            _userRepository = userRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<AssignSalesManagerResponseDto> AssignSalesToManagerAsync(AssignSalesManagerRequestDto dto,int adminId)
        {
            var salesUser = await _userRepository.GetByIdAsync(dto.SalesUserId)
                ?? throw new KeyNotFoundException("Sales user not found");

            if (salesUser.UserRole != UserRole.Sales)
                throw new InvalidOperationException("User is not a Sales role");

            if (salesUser.Status != UserStatus.Active)
                throw new InvalidOperationException("Sales user must be Active");

            var managerUser = await _userRepository.GetByIdAsync(dto.ManagerUserId)
                ?? throw new KeyNotFoundException("Manager user not found");

            if (managerUser.UserRole != UserRole.Manager)
                throw new InvalidOperationException("User is not a Manager role");

            if (managerUser.Status != UserStatus.Active)
                throw new InvalidOperationException("Manager must be Active");

            var existingAssignment =
                await _assignmentRepository.GetActiveBySalesUserIdAsync(dto.SalesUserId);

            if (existingAssignment != null &&
                existingAssignment.ManagerUserId == dto.ManagerUserId)
            {
                throw new InvalidOperationException(
                    "Sales user already assigned to this manager"
                );
            }

            if (existingAssignment != null)
            {
                existingAssignment.Deactivate(adminId);
                await _assignmentRepository.UpdateAsync(existingAssignment);

            }

            var assignment = AssignUsers.Create(
                dto.SalesUserId,
                dto.ManagerUserId,
                adminId
            );

            await _assignmentRepository.AddAsync(assignment);

            return new AssignSalesManagerResponseDto
            {
                SalesUserId = salesUser.UserId,
                SalesUserName = salesUser.FullName,
                SalesUserEmail = salesUser.Email,

                ManagerUserId = managerUser.UserId,
                ManagerName = managerUser.FullName,
                ManagerEmail = managerUser.Email
            };
        }


        public async Task<ManagerWithSalesResponseDto> GetManagerWithSalesAsync(int managerUserId)
        {
            var manager = await _userRepository.GetByIdAsync(managerUserId)
                ?? throw new KeyNotFoundException("Manager not found");

            if (manager.UserRole != UserRole.Manager)
                throw new InvalidOperationException("User is not a Manager");

            var salesUsers =
                await _assignmentRepository.GetSalesByManagerIdAsync(managerUserId);

            if (salesUsers == null || !salesUsers.Any())
                throw new InvalidOperationException("No sales users assigned to this manager");


            return new ManagerWithSalesResponseDto
            {
                ManagerUserId = manager.UserId,
                ManagerName = manager.FullName,
                SalesUsers = salesUsers
            };
        }

        public async Task<SalesWithManagerResponseDto> GetSalesWithManagerAsync(int salesUserId)
        {
            var salesUser = await _userRepository.GetByIdAsync(salesUserId)
                ?? throw new KeyNotFoundException("Sales user not found");

            if (salesUser.UserRole != UserRole.Sales)
                throw new InvalidOperationException("User is not a Sales role");

            var assignment =
                await _assignmentRepository.GetActiveBySalesUserIdAsync(salesUserId)
                ?? throw new InvalidOperationException("Sales user not assigned to any manager");

            var manager = await _userRepository.GetByIdAsync(assignment.ManagerUserId)
                ?? throw new KeyNotFoundException("Manager not found");

            return new SalesWithManagerResponseDto
            {
                SalesUserId = salesUser.UserId,
                SalesUserName = salesUser.FullName,
                ManagerUserId = manager.UserId,
                ManagerName = manager.FullName,
                ManagerEmail = manager.Email
            };
        }
        public async Task<AssignSalesManagerResponseDto> ChangeSalesManagerAsync(int salesUserId, int newManagerUserId, int adminId)
        {
            var salesUser = await _userRepository.GetByIdAsync(salesUserId)
                ?? throw new KeyNotFoundException("Sales user not found");

            if (salesUser.UserRole != UserRole.Sales)
                throw new InvalidOperationException("User is not a Sales role");

            var newManager = await _userRepository.GetByIdAsync(newManagerUserId)
                ?? throw new KeyNotFoundException("Manager not found");

            if (newManager.UserRole != UserRole.Manager)
                throw new InvalidOperationException("User is not a Manager");

            if (salesUser.Status != UserStatus.Active)
                throw new InvalidOperationException("Sales user must be Active");

            if (newManager.Status != UserStatus.Active)
                throw new InvalidOperationException("Manager must be Active");


            var existingAssignment = await _assignmentRepository.GetActiveBySalesUserIdAsync(salesUserId);
            if (existingAssignment != null)
            {
                existingAssignment.Deactivate(adminId);
                await _assignmentRepository.UpdateAsync(existingAssignment);
            }

            var newAssignment = AssignUsers.Create(salesUserId, newManagerUserId, adminId);
            await _assignmentRepository.AddAsync(newAssignment);

            return new AssignSalesManagerResponseDto
            {
                SalesUserId = salesUser.UserId,
                SalesUserName = salesUser.FullName,
                SalesUserEmail = salesUser.Email,

                ManagerUserId = newManager.UserId,
                ManagerName = newManager.FullName,
                ManagerEmail = newManager.Email
            };
        }

        public async Task DeleteAssignmentAsync(int assignmentId, int adminId)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(assignmentId)
                ?? throw new KeyNotFoundException("Assignment not found");

            assignment.Deactivate(adminId);
            await _assignmentRepository.UpdateAsync(assignment);
            assignment.Delete(adminId);
            await _assignmentRepository.UpdateAsync(assignment);

        }
        public async Task<ManagerWithSalesResponseDto> ReassignManagerTeamAsync(
     int oldManagerUserId,
     int newManagerUserId,
     int adminId)
        {
            if (oldManagerUserId == newManagerUserId)
                throw new InvalidOperationException("Both managers cannot be the same");

            var oldManager = await _userRepository.GetByIdAsync(oldManagerUserId)
                ?? throw new KeyNotFoundException("Old manager not found");

            var newManager = await _userRepository.GetByIdAsync(newManagerUserId)
                ?? throw new KeyNotFoundException("New manager not found");

            if (oldManager.UserRole != UserRole.Manager ||
                newManager.UserRole != UserRole.Manager)
                throw new InvalidOperationException("Invalid manager role");

            if (newManager.Status != UserStatus.Active)
                throw new InvalidOperationException("New manager must be Active");

            var teamAssignments =
                await _assignmentRepository.GetActiveByManagerIdAsync(oldManagerUserId);

            if (!teamAssignments.Any())
                throw new InvalidOperationException("Old manager has no active sales team");

            var reassignedSales = new List<SalesUserDto>();

            foreach (var assignment in teamAssignments)
            {
                assignment.Deactivate(adminId);
                await _assignmentRepository.UpdateAsync(assignment);

                var newAssignment = AssignUsers.Create(
                    assignment.SalesUserId,
                    newManagerUserId,
                    adminId);

                await _assignmentRepository.AddAsync(newAssignment);

                var salesUser = await _userRepository.GetByIdAsync(assignment.SalesUserId);

                reassignedSales.Add(new SalesUserDto
                {
                    SalesUserId = salesUser.UserId,
                    SalesUserName = salesUser.FullName,
                    Email = salesUser.Email
                });
            }

            return new ManagerWithSalesResponseDto
            {
                ManagerUserId = newManager.UserId,
                ManagerName = newManager.FullName,
                SalesUsers = reassignedSales
            };
        }

    }
}


