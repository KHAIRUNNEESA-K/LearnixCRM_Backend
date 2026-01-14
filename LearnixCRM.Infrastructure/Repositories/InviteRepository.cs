using Dapper;
using LearnixCRM.Application.Interfaces;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Infrastructure.Repositories
{
    public class InviteRepository : IInviteRepository
    {
        private readonly LearnixDbContext _context;
        private readonly IDbConnection _db;

        public InviteRepository(LearnixDbContext context, IDbConnection db)
        {
            _context = context;
            _db = db;
        }

        public async Task<UserInvite?> GetInviteByTokenAsync(string token)
        {
            var invite = await _db.QueryFirstOrDefaultAsync<UserInvite>(
                "sp_GetInviteByToken",
                new { Token = token },
                commandType: CommandType.StoredProcedure
            );

            return invite;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _db.QueryFirstOrDefaultAsync<User>(
                "sp_GetUserByEmail",
                new { Email = email },
                commandType: CommandType.StoredProcedure
            );

            return user;
        }

        public async Task SaveInviteAsync(UserInvite invite)
        {
            _context.UserInvites.Update(invite);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
