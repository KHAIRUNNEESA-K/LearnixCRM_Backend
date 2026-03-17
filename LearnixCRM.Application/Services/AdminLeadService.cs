using LearnixCRM.Application.DTOs.Lead;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Domain.Enum;

namespace LearnixCRM.Application.Services
{
    public class AdminLeadService : IAdminLeadService
    {
        private readonly IAdminLeadRepository _leadRepository;
        private readonly ITeamRepository _teamRepository;

        public AdminLeadService(
            IAdminLeadRepository leadRepository,
            ITeamRepository teamRepository)
        {
            _leadRepository = leadRepository;
            _teamRepository = teamRepository;
        }

        public async Task<IEnumerable<AdminLeadViewDto>> GetAllLeadsAsync()
        {
            var leads = await _leadRepository.GetAllLeadsAsync();

            if (leads == null || !leads.Any())
                throw new InvalidOperationException("No leads found");

            return leads;
        }

        public async Task<IEnumerable<AdminLeadViewDto>> GetLeadsByStatusAsync(LeadStatus status)
        {
            if (!Enum.IsDefined(typeof(LeadStatus), status))
                throw new InvalidOperationException("Invalid lead status");

            var leads = await _leadRepository.GetLeadsByStatusAsync((int)status);

            if (leads == null || !leads.Any())
                throw new InvalidOperationException($"No leads found for status {status}");

            return leads;
        }

        public async Task<IEnumerable<AdminLeadViewDto>> GetLeadsByTeamAsync(int teamId)
        {
            if (teamId <= 0)
                throw new InvalidOperationException("Invalid team id");

            var team = await _teamRepository.GetTeamByIdAsync(teamId);

            if (team == null)
                throw new KeyNotFoundException("Team not found");

            if (!team.IsActive)
                throw new InvalidOperationException("Team is inactive");

            var leads = await _leadRepository.GetLeadsByTeamAsync(teamId);

            if (leads == null || !leads.Any())
                throw new InvalidOperationException("No leads found for this team");

            return leads;
        }
    }
}