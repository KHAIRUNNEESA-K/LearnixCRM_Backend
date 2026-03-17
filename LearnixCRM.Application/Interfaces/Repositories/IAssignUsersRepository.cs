using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Repositories
{
    public interface IAssignUsersRepository
    {
        Task<AssignUsers?> GetActiveBySalesUserIdAsync(int salesUserId);
        Task<List<SalesUserDto>> GetSalesByTeamIdAsync(int teamId);
        Task AddAsync(AssignUsers assignment);
        Task UpdateAsync(AssignUsers assignment);
        Task<AssignUsers?> GetByIdAsync(int id);
        Task DeleteAsync(int id, int deletedBy);
        Task<List<AssignUsers>> GetActiveByTeamIdAsync(int teamId);
        Task DeactivateBySalesUserIdAsync(int salesUserId, int adminId);
    }
}
