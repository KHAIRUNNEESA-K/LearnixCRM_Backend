using LearnixCRM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces
{
    public interface IAdminService
    {
        Task<UserResponseDto> InviteUserAsync(CreateUserRequestDto dto, string adminName);
        Task<IEnumerable<UserResponseDto>> GetPendingUsersAsync();
        Task ActivateUserAsync(int userId, string password, string adminName);
        Task DeactivateUserAsync(int userId, string adminName);
        Task ChangeUserRoleAsync(int userId, string newRole, string adminName);
        Task ResendInviteAsync(int userId, string adminName);
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task ActivateUserAsync(int userId, string adminName);
        Task DeleteUserAsync(int userId, string adminName);
        Task<IEnumerable<UserInviteDto>> GetPendingInvitesAsync();

    }
}

