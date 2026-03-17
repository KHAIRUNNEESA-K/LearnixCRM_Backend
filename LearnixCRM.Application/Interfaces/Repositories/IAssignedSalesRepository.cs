using LearnixCRM.Application.DTOs.Team;
using LearnixCRM.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Repositories
{
    public interface IAssignedSalesRepository
    {
        Task<IEnumerable<SalesUserDto>> GetSalesByManagerAsync(int managerUserId);

        Task<SalesUserDto?> GetSalesByManagerAndSalesIdAsync(int managerUserId, int salesUserId);

        Task<IEnumerable<ManagerTeamDto>> GetTeamsByManagerAsync(int managerUserId);

        Task<IEnumerable<SalesUserDto>> GetTeamMembersAsync(int teamId, int managerUserId);
    }
}
