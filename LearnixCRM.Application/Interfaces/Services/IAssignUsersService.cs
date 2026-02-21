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
        Task<AssignSalesManagerResponseDto> AssignSalesToManagerAsync(AssignSalesManagerRequestDto dto, int adminId);
        Task<ManagerWithSalesResponseDto> GetManagerWithSalesAsync(int managerUserId);
        Task<SalesWithManagerResponseDto> GetSalesWithManagerAsync(int salesUserId);
        Task<AssignSalesManagerResponseDto> ChangeSalesManagerAsync(int salesUserId, int newManagerUserId, int adminId);
        Task DeleteAssignmentAsync(int assignmentId, int adminId);
        Task<ManagerWithSalesResponseDto> ReassignManagerTeamAsync(int oldManagerUserId,int newManagerUserId, int adminId);


    }
}
