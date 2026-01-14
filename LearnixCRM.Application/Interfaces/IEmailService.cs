using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendInviteAsync(string toEmail, string inviteLink);
        Task SendResetPasswordAsync(string toEmail, string resetLink);
    }
}
