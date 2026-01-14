using LearnixCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces
{
    public interface IInviteService
    {
        Task<UserInvite?> ValidateInviteAsync(string token);
        Task AcceptInviteAsync(string token, string password);
    }
}
