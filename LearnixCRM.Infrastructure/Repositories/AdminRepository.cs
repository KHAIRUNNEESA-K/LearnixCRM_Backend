using Dapper;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;
using LearnixCRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LearnixCRM.Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly LearnixDbContext _context;
        private readonly IDbConnection _db;

        public AdminRepository(LearnixDbContext context, IDbConnection db)
        {
            _context = context;
            _db = db;
        }


        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _db.QueryAsync<User>(
                "sp_GetAllUsers",
                commandType: CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _db.QueryAsync<User>(
                "sp_GetActiveUsers",
                commandType: CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<User>> GetPendingUsersAsync()
        {
            return await _db.QueryAsync<User>(
                "sp_GetPendingUsers",
                commandType: CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<User>> GetInactiveUserAsync()
        {
            return await _db.QueryAsync<User>(
                "sp_GetInactiveUsers",
                commandType: CommandType.StoredProcedure);
        }
            
        public async Task<IEnumerable<User>> GetBlockedUserAsync()
        {
            return await _db.QueryAsync<User>(
                "sp_GetBlockedUsers",
                commandType: CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<User>> GetRejectedUserAsync()
        {
            return await _db.QueryAsync<User>(
                "sp_GetRejectedUsers",
                commandType: CommandType.StoredProcedure);
        }
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId, DbType.Int32);

            var user = await _db.QueryFirstOrDefaultAsync<User>(
                "sp_GetUserById",
                parameters,
                commandType: CommandType.StoredProcedure);

            return user;
        }
        public async Task<User?> GetActiveUserByIdAsync(int userId)
        {
            return await _db.QueryFirstOrDefaultAsync<User>(
                "sp_GetActiveUserById",
                new { UserId = userId },
                commandType: CommandType.StoredProcedure);
        }


        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId, int deletedBy)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId && u.DeletedAt == null);

            user.Delete(deletedBy);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<User>> GetUsersByRoleAndStatusAsync(UserRole role, UserStatus status)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@Role", (int)role);
            parameters.Add("@Status", (int)status);

            return await _db.QueryAsync<User>(
                "sp_GetUsersByRoleAndStatus",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<User>> GetApprovedUsersPendingPasswordAsync()
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Status", (int)UserStatus.Approved);

            return await _db.QueryAsync<User>(
                "sp_GetUsersByStatusAndPasswordState",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }



    }

}
