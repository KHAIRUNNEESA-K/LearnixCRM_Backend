using LearnixCRM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<LoginUserDto> GetUserForLoginAsync(string email);
    }
}
