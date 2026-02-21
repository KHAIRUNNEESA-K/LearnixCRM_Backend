using Dapper;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LearnixCRM.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly LearnixDbContext _context;
        private readonly IDbConnection _db;

        public RefreshTokenRepository(LearnixDbContext context, IDbConnection db)
        {
            _context = context;
            _db = db;
        }

        public async Task SaveAsync(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _db.QueryFirstOrDefaultAsync<RefreshToken>(
                "sp_GetRefreshTokenByToken",
                new { Token = token },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateAsync(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveByUserIdAsync(int userId)
        {
            var tokens = _context.RefreshTokens
                .Where(x => x.UserId == userId);

            _context.RefreshTokens.RemoveRange(tokens);
            await _context.SaveChangesAsync();
        }

        public async Task RevokeAsync(string token, int userId)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == token);
            var userCheck = await _context.Users
                            .IgnoreQueryFilters()
                            .FirstOrDefaultAsync(u => u.UserId == refreshToken.UserId);

            Console.WriteLine("User exists in DB? " + (userCheck != null));
            Console.WriteLine("DeletedAt: " + userCheck?.DeletedAt);
            Console.WriteLine("Status: " + userCheck?.Status);

            if (refreshToken != null)
            {
                refreshToken.Revoke(userId);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RevokeAllByUserIdAsync(int userId)
        {
            var tokens = await _context.RefreshTokens
                        .Where(x => x.UserId == userId)
                        .ToListAsync();


            _context.RefreshTokens.RemoveRange(tokens);
            await _context.SaveChangesAsync();



        }
        public async Task<User?> GetByIdForAuthAsync(int id)
        {
            return await _context.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

    }
}
