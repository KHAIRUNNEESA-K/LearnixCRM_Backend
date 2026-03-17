using Dapper;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LearnixCRM.Infrastructure.Repositories
{
    public class AssignUsersRepository : IAssignUsersRepository
    {
        private readonly LearnixDbContext _context;
        private readonly IDbConnection _db;

        public AssignUsersRepository(
            LearnixDbContext context,
            IDbConnection db)
        {
            _context = context;
            _db = db;
        }

        public async Task<AssignUsers?> GetActiveBySalesUserIdAsync(int salesUserId)
        {
            return await _db.QueryFirstOrDefaultAsync<AssignUsers>(
                "sp_GetActiveSalesManager",
                new { SalesUserId = salesUserId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<List<SalesUserDto>> GetSalesByTeamIdAsync(int teamId)
        {
            var result = await _db.QueryAsync<SalesUserDto>(
                "sp_GetSalesByTeamId",
                new { TeamId = teamId },
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        public async Task AddAsync(AssignUsers assignment)
        {
            await _context.AssignUsers.AddAsync(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AssignUsers assignment)
        {
            _context.AssignUsers.Update(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task<AssignUsers?> GetByIdAsync(int id)
        {
            return await _db.QueryFirstOrDefaultAsync<AssignUsers>(
                "sp_GetSalesManagerAssignmentById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeleteAsync(int id, int deletedBy)
        {
            var assignment = await _context.AssignUsers
                .FirstOrDefaultAsync(x => x.AssignId == id && x.DeletedAt == null);

            if (assignment != null)
            {
                assignment.Deactivate(deletedBy);
                _context.AssignUsers.Update(assignment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<AssignUsers>> GetActiveByTeamIdAsync(int teamId)
        {
            return await _context.AssignUsers
                .Where(x =>
                    x.TeamId == teamId &&
                    x.IsActive &&
                    x.DeletedAt == null)
                .ToListAsync();
        }
        public async Task DeactivateBySalesUserIdAsync(int salesUserId, int adminId)
        {
            var assignment = await _context.AssignUsers
                .FirstOrDefaultAsync(x =>
                    x.SalesUserId == salesUserId &&
                    x.IsActive &&
                    x.DeletedAt == null);

            if (assignment != null)
            {
                assignment.Deactivate(adminId);
                _context.AssignUsers.Update(assignment);
                await _context.SaveChangesAsync();
            }
        }
    }
}