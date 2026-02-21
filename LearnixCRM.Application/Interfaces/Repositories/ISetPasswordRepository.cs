using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Repositories
{
    public interface ISetPasswordRepository
    {
        Task CreateAsync(UserPasswordToken token);
        Task<UserPasswordToken?> GetValidTokenAsync(string tokenHash);
        Task UpdateAsync(UserPasswordToken token);
        Task InvalidateExistingTokensAsync(int userId, PasswordTokenType tokenType, int updatedBy);
        Task<UserPasswordToken?> GetActiveTokenAsync(int userId, PasswordTokenType tokenType);
        Task DeleteExpiredAsync();

    }
}
