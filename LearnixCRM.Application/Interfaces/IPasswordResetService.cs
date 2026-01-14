using LearnixCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces
{
    public interface IPasswordResetService
    {
        Task RequestResetAsync(string email, string requestedBy);
        Task<PasswordReset?> ValidateTokenAsync(string token);
        Task ResetPasswordAsync(string token, string password);
    }
}
