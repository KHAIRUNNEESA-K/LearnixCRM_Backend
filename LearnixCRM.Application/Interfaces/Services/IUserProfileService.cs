using LearnixCRM.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Services
{
    public interface IUserProfileService
    {
        Task<UserProfileDto> GetProfileAsync(int userId);
        Task<UserProfileDto> UpdateProfileAsync(int userId, UpdateProfileDto dto);
    }
}
