using AutoMapper;
using LearnixCRM.Application.DTOs.Lead;
using LearnixCRM.Application.Interfaces;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Domain.Constants;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LearnixCRM.Application.Services
{
    public class SalesLeadService : ISalesLeadService
    {
        private readonly ISalesLeadRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAssignUsersRepository _assignUsersRepository;
        public SalesLeadService(ISalesLeadRepository repository, IMapper mapper,IAssignUsersRepository assignUsersRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _assignUsersRepository = assignUsersRepository;
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
            var assignment = await _assignUsersRepository.GetActiveBySalesUserIdAsync(salesUserId);

            if (assignment == null || !assignment.IsActive)
                throw new InvalidOperationException("Sales user must be assigned to an active team.");

            var lead = new Lead(
                dto.FullName,
                dto.Email,
                dto.Phone,
                dto.CourseId,
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

            if (existing.Status == LeadStatus.Converted)
                throw new InvalidOperationException("Converted lead cannot be updated.");

            if (!string.IsNullOrWhiteSpace(dto.FullName) ||
                !string.IsNullOrWhiteSpace(dto.Email) ||
                !string.IsNullOrWhiteSpace(dto.Phone) ||
                dto.CourseId.HasValue ||
                dto.Source.HasValue)
            {
                existing.UpdateLead(
                    dto.FullName ?? existing.FullName,
                    dto.Email ?? existing.Email,
                    dto.Phone ?? existing.Phone,
                    dto.CourseId ?? existing.CourseId,
                    dto.Source ?? existing.Source,
                    salesUserId
                );
            }

            if (dto.Status.HasValue)
            {
                if (existing.Status == LeadStatus.Converted)
                    throw new InvalidOperationException("Cannot change status of a converted lead.");

                existing.UpdateStatus(dto.Status.Value, salesUserId);
            }

            if (!string.IsNullOrWhiteSpace(dto.Remark))
            {
                existing.UpdateRemark(dto.Remark, salesUserId);

                var remarkText = dto.Remark.ToLower();

                if (remarkText.Contains(LeadRemarks.Blacklist.ToLower()) ||
                    remarkText.Contains(LeadRemarks.Spam.ToLower()) ||
                    remarkText.Contains(LeadRemarks.Invalid.ToLower()))
                {
                    await MoveLeadToBlacklistAsync(existing, salesUserId);
                    return null;
                }

                if (remarkText.Contains(LeadRemarks.Converted.ToLower()) ||
                    remarkText.Contains("enrolled"))
                {
                    await MoveLeadToStudentAsync(existing, salesUserId);
                    return _mapper.Map<LeadResponseDto>(existing);
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

            await _repository.AddBlacklistAsync(blacklist);
            await _repository.DeleteAsync(lead);
        }

        private async Task MoveLeadToStudentAsync(Lead lead, int userId)
        {
            var student = new Student(
                lead,
                DateTime.UtcNow,
                userId
            );

            await _repository.AddStudentAsync(student);

            lead.UpdateStatus(LeadStatus.Converted, userId);

            await _repository.UpdateAsync(lead);
        }
    }
}