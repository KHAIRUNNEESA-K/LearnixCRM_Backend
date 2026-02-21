using LearnixCRM.Application.DTOs.SetPasswordToken;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Services
{
    public interface ISetPasswordService
    {
        Task<SetPasswordResponseDto> SetPasswordAsync(SetPasswordRequestDto dto);
    }
}
