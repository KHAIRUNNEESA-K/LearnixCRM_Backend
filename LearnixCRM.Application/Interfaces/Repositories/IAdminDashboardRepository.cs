using LearnixCRM.Application.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Repositories
{
    public interface IAdminDashboardRepository
    {
        Task<AdminDashboardDto> GetAdminDashboardAsync();
    }
}
