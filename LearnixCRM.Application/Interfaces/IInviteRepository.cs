using LearnixCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces
{
    public interface IInviteRepository
    {
        Task<UserInvite?> GetInviteByTokenAsync(string token);
        Task<User?> GetUserByEmailAsync(string email); 
        Task SaveInviteAsync(UserInvite invite);
        Task UpdateUserAsync(User user);
    }
}
