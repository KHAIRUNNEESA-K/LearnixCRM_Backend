using LearnixCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task SaveAsync(RefreshToken token);
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task UpdateAsync(RefreshToken token);
        Task RemoveByUserIdAsync(int userId);
        Task RevokeAsync(string token, int userId);
        Task RevokeAllByUserIdAsync(int userId);
        Task<User?> GetByIdForAuthAsync(int id);
    }
}
