using LearnixCRM.Application.DTOs.Lead;
using LearnixCRM.Domain.Enum;

public interface ISalesLeadService
{
    Task<IEnumerable<LeadResponseDto>> GetAllLeadsAsync();

    Task<LeadResponseDto> GetLeadByIdAsync(int leadId);

    Task<IEnumerable<LeadResponseDto>> GetLeadsByStatusAsync(LeadStatus status);

    Task<LeadResponseDto> AddLeadAsync(CreateLeadRequestDto dto, int salesUserId);

    Task<LeadResponseDto> UpdateLeadAsync(UpdateLeadRequestDto dto, int salesUserId);

    Task DeleteLeadAsync(int leadId, int salesUserId);
}