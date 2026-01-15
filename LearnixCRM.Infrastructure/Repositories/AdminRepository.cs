using Dapper;
using LearnixCRM.Application.Interfaces;
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
           return await _db.QueryAsync<User>("sp_GetAllUsers",commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<User>> GetPendingUsersAsync()
        {
            return await _db.QueryAsync<User>("sp_GetPendingUsers",commandType: CommandType.StoredProcedure);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);

            try
            {

                await _context.SaveChangesAsync();
                return user;
            }
            catch (DbUpdateException ex)
            {

                var innerMessage = ex.InnerException?.Message;
                Console.WriteLine("Database error: " + innerMessage);

                throw new Exception("Unable to save user. " + innerMessage);
            }
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task CreateInviteAsync(UserInvite invite)
        {
            _context.UserInvites.Add(invite);
            await _context.SaveChangesAsync();
        }

        public async Task<UserInvite?> GetInviteByEmailAsync(string email)
            => await _context.UserInvites
                .FirstOrDefaultAsync(x => x.Email == email);

        public async Task SaveInviteAsync(UserInvite invite)
        {
            _context.UserInvites.Update(invite);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId, string deletedBy)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (user.UserRole == UserRole.Admin)
            {
                throw new Exception("Admin user cannot be deleted");
            }

            user.Delete(deletedBy);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserInvite>> GetPendingInvitesAsync()
        {
            return await _db.QueryAsync<UserInvite>(
                "sp_GetPendingInvites",
                commandType: CommandType.StoredProcedure);
        }
    }
}
