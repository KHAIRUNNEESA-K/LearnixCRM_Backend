using AutoMapper;
using LearnixCRM.Application.Common.Responses;
using LearnixCRM.Application.DTOs.Lead;
using LearnixCRM.Application.Interfaces;
using LearnixCRM.Domain.Constants;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Services
{
    public class SalesLeadService : ISalesLeadService
    {
        private readonly ISalesLeadRepository _repository;
        private readonly IMapper _mapper;

        public SalesLeadService(ISalesLeadRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LeadResponseDto>> GetAllLeadsAsync()
        {
            var leads = await _repository.GetAllAsync();

            if (!leads.Any())
                throw new KeyNotFoundException("No leads found for this sales user.");

            return _mapper.Map<IEnumerable<LeadResponseDto>>(leads);
        }

        public async Task<LeadResponseDto> GetLeadByIdAsync(int leadId)
        {
            var lead = await _repository.GetByIdAsync(leadId);

            if (lead == null)
                throw new KeyNotFoundException($"Lead with ID {leadId} not found.");

            return _mapper.Map<LeadResponseDto>(lead);
        }

        public async Task<IEnumerable<LeadResponseDto>> GetLeadsByStatusAsync(LeadStatus status)
        {
            var leads = await _repository.GetByStatusAsync(status);

            if (!leads.Any())
                throw new KeyNotFoundException($"No leads found with status {status}.");

            return _mapper.Map<IEnumerable<LeadResponseDto>>(leads);
        }

        public async Task<LeadResponseDto> AddLeadAsync(CreateLeadRequestDto dto, int salesUserId)
        {
            var lead = new Lead(
                dto.FullName,
                dto.Email,
                dto.Phone,
                dto.CourseInterested,
                dto.Source,
                salesUserId,
                salesUserId 
            );

            await _repository.AddAsync(lead);

            return _mapper.Map<LeadResponseDto>(lead);
        }


        public async Task<LeadResponseDto> UpdateLeadAsync(UpdateLeadRequestDto dto, int salesUserId)
        {
            var existing = await _repository.GetByIdAsync(dto.LeadId);

            if (existing == null)
                throw new KeyNotFoundException($"Lead with ID {dto.LeadId} not found.");

            existing.UpdateLead(
                dto.FullName,
                dto.Email,
                dto.Phone,
                dto.CourseInterested,
                dto.Source,
                salesUserId
            );

 
            if (dto.Status.HasValue)
                existing.UpdateStatus(dto.Status.Value, salesUserId);

            if (!string.IsNullOrWhiteSpace(dto.Remark))
                existing.UpdateRemark(dto.Remark, salesUserId);

         
            if (!string.IsNullOrWhiteSpace(dto.Remark))
            {
                switch (dto.Remark)
                {
                    case LeadRemarks.Blacklist:
                    case LeadRemarks.Spam:
                    case LeadRemarks.Invalid:
                        await MoveLeadToBlacklistAsync(existing, salesUserId);
                        return null;

                    case LeadRemarks.Converted:
                        await MoveLeadToStudentAsync(existing, salesUserId);
                        return null;
                }
            }

            await _repository.UpdateAsync(existing);

            return _mapper.Map<LeadResponseDto>(existing);
        }

        public async Task DeleteLeadAsync(int leadId, int salesUserId)
        {
            var existing = await _repository.GetByIdAsync(leadId);

            if (existing == null)
                throw new KeyNotFoundException($"Lead with ID {leadId} not found.");

            await _repository.DeleteAsync(existing);
        }
        private async Task MoveLeadToBlacklistAsync(Lead lead, int userId)
        {
            var blacklist = new Blacklist(
                lead.Email,
                lead.Phone,
                lead.Remark ?? "Unknown",
                userId
            );

            await _repository.AddBlacklistAsync(blacklist); // Use repository
            await _repository.DeleteAsync(lead);
        }

        private async Task MoveLeadToStudentAsync(Lead lead, int userId)
        {
            var student = new Student(
                lead,
                DateTime.UtcNow,
                userId
            );

            await _repository.AddStudentAsync(student); // Use repository
            await _repository.DeleteAsync(lead);
        }
    }
}
