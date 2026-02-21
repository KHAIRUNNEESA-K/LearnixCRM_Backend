using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendUserRegisteredAsync(string toEmail);
        Task SendApprovalEmailAsync(string toEmail, string setPasswordLink);
        Task SendRejectionEmailAsync(string toEmail, string rejectReason);
        Task SendResetPasswordEmailAsync(string toEmail, string resetPasswordLink);

    }
}
