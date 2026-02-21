using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;

namespace LearnixCRM.Application.Interfaces.Repositories
{
    public interface IAdminRepository
    {
       
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetActiveUsersAsync();

        Task<IEnumerable<User>> GetPendingUsersAsync();
        Task<User?> GetActiveUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetInactiveUserAsync();
        Task<IEnumerable<User>> GetRejectedUserAsync();
        Task<User?> GetUserByIdAsync(int userId);
        Task UpdateUserAsync(User user); 
        Task DeleteUserAsync(int userId, int deletedBy);
        Task<IEnumerable<User>> GetUsersByRoleAndStatusAsync(UserRole role,UserStatus status);
        Task<IEnumerable<User>> GetBlockedUserAsync();
        Task<IEnumerable<User>> GetApprovedUsersPendingPasswordAsync();

    }
}
