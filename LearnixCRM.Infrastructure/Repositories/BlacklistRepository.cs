using Dapper;
using LearnixCRM.Application.Interfaces;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LearnixCRM.Infrastructure.Repositories
{
    public class BlacklistRepository : IBlacklistRepository
    {
        private readonly IDbConnection _db;
        private readonly int _currentUserId;
        private readonly LearnixDbContext _context;

        public BlacklistRepository(IDbConnection db, IHttpContextAccessor accessor,LearnixDbContext context)
        {
            _db = db;
            _context = context;

            var userId = accessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _currentUserId = int.Parse(userId!);
        }

        public async Task<IEnumerable<Blacklist>> GetAllAsync()
        {
            return await _db.QueryAsync<Blacklist>(
                "sp_GetAllBlacklists",
                new { },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Blacklist?> GetByIdAsync(int id)
        {
            return await _db.QueryFirstOrDefaultAsync<Blacklist>(
                "sp_GetBlacklistById",
                new { BlacklistId = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Blacklist?> GetByEmailOrPhoneAsync(string email, string? phone)
        {
            return await _db.QueryFirstOrDefaultAsync<Blacklist>(
                "sp_GetBlacklistByEmailOrPhone",
                new { Email = email, Phone = phone },
                commandType: CommandType.StoredProcedure);
        }
        public async Task<bool> ExistsInBlacklistAsync(string? email, string? phone)
        {
            return await _context.Blacklists
                .AnyAsync(x =>
                    x.DeletedAt == null &&
                    (
                        (email != null && x.Email == email) ||
                        (phone != null && x.Phone == phone)
                    )
                );
        }
    }
}
