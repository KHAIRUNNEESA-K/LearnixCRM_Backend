using LearnixCRM.Application.DTOs.AssignUsers;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Services
{
    public interface IAssignUsersService
    {
        Task<AssignSalesManagerResponseDto> AssignSalesToTeamAsync(AssignSalesManagerRequestDto dto, int adminId);
        Task<ManagerWithSalesResponseDto> GetManagerWithSalesAsync(int managerUserId);
        Task RemoveSalesFromTeamAsync(int assignmentId, int adminId);
        Task<AssignSalesManagerResponseDto> ChangeSalesTeamAsync(int salesUserId,int newTeamId, int adminId);

    }
}
