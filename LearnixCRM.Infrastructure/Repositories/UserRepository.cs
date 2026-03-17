using Dapper;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LearnixDbContext _context;
        private readonly IDbConnection _db;

        public UserRepository(LearnixDbContext context, IDbConnection db)
        {
            _context = context;
            _db = db;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.QueryFirstOrDefaultAsync<User>(
                "sp_GetUserByEmail",
                new { Email = email },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _db.QueryFirstOrDefaultAsync<User>(
                "sp_GetUserById",
                new { UserId = userId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }

}
