using LearnixCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Repositories
{
    public interface IUserProfileRepository
    {
        Task<User?> GetProfileByUserIdAsync(int userId);
        Task UpdateProfileAsync(User user);
    }
}
