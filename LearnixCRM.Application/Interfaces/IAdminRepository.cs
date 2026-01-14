using LearnixCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces
{
       public interface IAdminRepository
        {
            Task<User> CreateUserAsync(User user);
            Task<User?> GetByIdAsync(int userId);
            Task UpdateUserAsync(User user);

            Task<IEnumerable<User>> GetAllUsersAsync();
            Task<IEnumerable<User>> GetPendingUsersAsync();

            Task CreateInviteAsync(UserInvite invite);
            Task<UserInvite?> GetInviteByEmailAsync(string email);
            Task SaveInviteAsync(UserInvite invite);
            Task DeleteUserAsync(int userId, string deletedBy);
            Task<IEnumerable<UserInvite>> GetPendingInvitesAsync();

        }
}

