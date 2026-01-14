using LearnixCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces
{
    public interface IPasswordResetRepository
    {
        Task<PasswordReset?> GetByTokenAsync(string token);
        Task CreatePasswordResetAsync(string email, string token, string requestedBy);
        Task ResetPasswordAsync(string email, string passwordHash, string token);
        Task<User?> GetUserByEmailAsync(string email);

    }
}
