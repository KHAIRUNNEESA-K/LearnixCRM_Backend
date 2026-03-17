using LearnixCRM.Application.DTOs.Lead;
using LearnixCRM.Domain.Enum;

namespace LearnixCRM.Application.Interfaces.Services
{
    public interface IAdminLeadService
    {
        Task<IEnumerable<AdminLeadViewDto>> GetAllLeadsAsync();

        Task<IEnumerable<AdminLeadViewDto>> GetLeadsByStatusAsync(LeadStatus status);

        Task<IEnumerable<AdminLeadViewDto>> GetLeadsByTeamAsync(int teamId);
    }
}