using LearnixCRM.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<IEnumerable<UserResponseDto>> GetActiveUsersAsync();

        Task<UserResponseDto> GetUserByIdAsync(int userId);
        Task<UserResponseDto> GetActiveUserByIdAsync(int userId);
        Task<IEnumerable<UserResponseDto>> GetInactiveUsersAsync();
        Task<IEnumerable<RegisterUserResponseDto>> GetRejectedUsersAsync();

        Task<IEnumerable<RegisterUserResponseDto>> GetPendingUsersAsync();
        Task<RegisterUserResponseDto> ApproveUserAndSendTokenAsync(int userId, int adminId);
        Task<RegisterUserResponseDto> RejectUserAsync(int userId, int adminId, string rejectReason);
        Task<UserResponseDto> BlockUserAsync(int userId, int adminId);
        Task UnblockUserAsync(int userId, int adminId);
        Task DeleteUserAsync(int userId, int adminId);
        Task<IEnumerable<UserResponseDto>> GetActiveSalesExecutivesAsync();
        Task<IEnumerable<UserResponseDto>> GetActiveManagersAsync();
        Task<IEnumerable<UserResponseDto>> GetBlockedUsersAsync();
        Task ResendSetPasswordTokenAsync(string email, int adminId);
        Task<IEnumerable<UserResponseDto>> GetApprovedUsersPendingPasswordAsync();
    }
}
