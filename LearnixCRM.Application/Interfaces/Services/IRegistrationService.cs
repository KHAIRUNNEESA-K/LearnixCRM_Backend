using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Services
{
    public interface IRegistrationService
    {
        Task<RegisterUserResponseDto> RegisterAsync(RegisterUserRequestDto dto);
    }
}
