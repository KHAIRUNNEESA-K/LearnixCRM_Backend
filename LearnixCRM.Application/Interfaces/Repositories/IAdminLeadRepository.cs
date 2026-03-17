using LearnixCRM.Application.DTOs.Lead;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Repositories
{
    public interface IAdminLeadRepository
    {
        Task<IEnumerable<AdminLeadViewDto>> GetAllLeadsAsync();

        Task<IEnumerable<AdminLeadViewDto>> GetLeadsByStatusAsync(int status);

        Task<IEnumerable<AdminLeadViewDto>> GetLeadsByTeamAsync(int teamId);
    }
}
