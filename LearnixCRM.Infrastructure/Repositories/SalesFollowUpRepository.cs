using Dapper;
using LearnixCRM.Application.Interfaces;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LearnixCRM.Infrastructure.Repositories
{
    public class SalesFollowUpRepository : ISalesFollowUpRepository
    {
        private readonly IDbConnection _db;
        private readonly LearnixDbContext _context;
        private readonly int _currentUserId;

        public SalesFollowUpRepository(
            IDbConnection db,
            LearnixDbContext context,
            IHttpContextAccessor accessor)
        {
            _db = db;
            _context = context;

            var userId = accessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            _currentUserId = int.Parse(userId!);
        }

        public async Task<IEnumerable<FollowUp>> GetAllAsync(int salesUserId)
        {
            return await _db.QueryAsync<FollowUp>(
                "sp_GetFollowUpsBySalesUser",
                new { SalesUserId = salesUserId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<FollowUp?> GetByIdAsync(int followUpId, int salesUserId)
        {
            return await _db.QueryFirstOrDefaultAsync<FollowUp>(
                "sp_GetFollowUpByIdForSalesUser",
                new
                {
                    FollowUpId = followUpId,
                    SalesUserId = salesUserId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task AddAsync(FollowUp followUp)
        {
            _context.FollowUps.Add(followUp);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FollowUp followUp)
        {
            _context.FollowUps.Update(followUp);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(FollowUp followUp)
        {
            followUp.SetDeleted(_currentUserId);
            _context.FollowUps.Update(followUp);
            await _context.SaveChangesAsync();
        }
    }
}