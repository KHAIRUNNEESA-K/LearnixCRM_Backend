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
        Task<List<SalesUserDto>> GetSalesByManagerIdAsync(int managerUserId);
        Task AddAsync(AssignUsers assignment);
        Task UpdateAsync(AssignUsers assignment);
        Task<AssignUsers?> GetByIdAsync(int id);
        Task<bool> ManagerHasActiveSalesAsync(int managerUserId);
        Task<List<AssignUsers>> GetActiveByManagerIdAsync(int managerUserId);
    }
}
