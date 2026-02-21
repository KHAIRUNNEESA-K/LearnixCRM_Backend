using Dapper;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<SalesUserDto>> GetSalesByManagerIdAsync(int managerUserId)
        {
            var result = await _db.QueryAsync<SalesUserDto>(
                "sp_GetSalesByManagerId",
                new { ManagerUserId = managerUserId },
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

        public async Task DeleteAsync(int id, int updatedBy)
        {
            var assignment = await _context.AssignUsers
                .FirstOrDefaultAsync(x => x.AssignId == id && x.DeletedAt == null);

            if (assignment != null)
            {
                assignment.Deactivate(updatedBy);
                _context.AssignUsers.Update(assignment);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> ManagerHasActiveSalesAsync(int managerUserId)
        {
            return await _context.AssignUsers
                .AnyAsync(x =>
                    x.ManagerUserId == managerUserId &&
                    x.IsActive);
        }

        public async Task<List<AssignUsers>> GetActiveByManagerIdAsync(int managerUserId)
        {
            return await _context.AssignUsers
                .Where(x => x.ManagerUserId == managerUserId &&
                            x.IsActive)
                .ToListAsync();
        }



    }
}

