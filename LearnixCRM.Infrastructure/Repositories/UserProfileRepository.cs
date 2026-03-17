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
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly LearnixDbContext _context;
        private readonly IDbConnection _db;

        public UserProfileRepository(LearnixDbContext context, IDbConnection db)
        {
            _context = context;
            _db = db;
        }

        public async Task<User?> GetProfileByUserIdAsync(int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, DbType.Int32);

            return await _db.QueryFirstOrDefaultAsync<User>(
                "sp_GetUserProfileById",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateProfileAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
