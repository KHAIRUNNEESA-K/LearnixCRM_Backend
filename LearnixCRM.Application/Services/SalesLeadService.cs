using AutoMapper;
using LearnixCRM.Application.DTOs.Lead;
using LearnixCRM.Application.Interfaces;
using LearnixCRM.Application.Interfaces.Repositories;
using LearnixCRM.Domain.Constants;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;


namespace LearnixCRM.Application.Services
{
    public class SalesLeadService : ISalesLeadService
    {
        private readonly ISalesLeadRepository _repository;
        private readonly IMapper _mapper;
        private readonly IBlacklistRepository _blacklistRepository;
        private readonly ICourseRepository _courseRepository;
        public SalesLeadService(ISalesLeadRepository repository, IMapper mapper, IBlacklistRepository blacklistRepository,ICourseRepository courseRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _blacklistRepository = blacklistRepository;
            _courseRepository = courseRepository;
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
            
            if (await _blacklistRepository.ExistsInBlacklistAsync(dto.Email, dto.Phone))
            {
                throw new InvalidOperationException("This lead is blacklisted.");
            }

            
            var courseExists = await _courseRepository.ExistsAsync(dto.CourseId);
            if (!courseExists)
            {
                throw new InvalidOperationException("Selected Course does not exist.");
            }

            
            var leadExists = await _repository.ExistsByEmailOrPhoneAsync(dto.Email, dto.Phone);
            if (leadExists)
            {
                throw new InvalidOperationException("Lead with this Email or Phone already exists.");
            }

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
                if ((int)dto.Status.Value < (int)existing.Status)
                {
                    throw new InvalidOperationException("Lead status cannot move backward.");
                }

                existing.UpdateStatus(dto.Status.Value, salesUserId);
            }

            if (existing.Status == LeadStatus.Converted || existing.Status == LeadStatus.Lost)
            {
                throw new InvalidOperationException("Lead already closed and cannot be modified.");
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

                if (remarkText.Contains(LeadRemarks.Converted.ToLower()))
                {
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
            await _repository.DeleteAsync(lead);
        }
    }
}
