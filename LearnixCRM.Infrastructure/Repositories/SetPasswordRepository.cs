using Dapper;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;
using LearnixCRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LearnixCRM.Infrastructure.Repositories
{
    public class SetPasswordRepository : ISetPasswordRepository
    {
        private readonly LearnixDbContext _context;
        private readonly IDbConnection _db;

        public SetPasswordRepository(
            LearnixDbContext context,
            IDbConnection db)
        {
            _context = context;
            _db = db;
        }

        public async Task CreateAsync(UserPasswordToken token)
        {
            await _context.UserPasswordTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task<UserPasswordToken?> GetValidTokenAsync(string tokenHash)
        {
            return await _db.QueryFirstOrDefaultAsync<UserPasswordToken>(
                "sp_GetValidPasswordToken",
                new { TokenHash = tokenHash },
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateAsync(UserPasswordToken token)
        {
            _context.UserPasswordTokens.Update(token);
            await _context.SaveChangesAsync();
        }

        public async Task InvalidateExistingTokensAsync(int userId,PasswordTokenType tokenType, int updatedBy)
        {
            var tokens = await _context.UserPasswordTokens
                .Where(t =>
                    t.UserId == userId &&
                    t.TokenType == tokenType &&
                    !t.IsUsed &&
                    t.Expiry > DateTime.UtcNow)
                .ToListAsync();

            foreach (var token in tokens)
                token.MarkUsed(updatedBy);

            await _context.SaveChangesAsync();
        }

        public async Task<UserPasswordToken?> GetActiveTokenAsync(int userId, PasswordTokenType tokenType)
        {
            return await _context.UserPasswordTokens
                .Where(t =>
                    t.UserId == userId &&
                    t.TokenType == tokenType &&
                    !t.IsUsed &&
                    t.Expiry > DateTime.UtcNow)
                .OrderByDescending(t => t.CreatedAt)
                .FirstOrDefaultAsync();
        }


        public async Task DeleteExpiredAsync()
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-7);

            var tokens = await _context.UserPasswordTokens
                .Where(t =>
                    t.IsUsed ||
                    t.Expiry < DateTime.UtcNow ||
                    t.CreatedAt < cutoffDate)
                .ToListAsync();

            _context.UserPasswordTokens.RemoveRange(tokens);
            await _context.SaveChangesAsync();
        }

    }
}
