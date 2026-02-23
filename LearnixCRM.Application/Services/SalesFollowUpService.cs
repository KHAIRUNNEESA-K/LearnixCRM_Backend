using AutoMapper;
using LearnixCRM.Application.DTOs.FollowUp;
using LearnixCRM.Application.Interfaces;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Services
{
    public class SalesFollowUpService : ISalesFollowUpService
    {
        private readonly ISalesFollowUpRepository _repository;
        private readonly IMapper _mapper;

        public SalesFollowUpService(ISalesFollowUpRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FollowUpResponseDto>> GetAllFollowUpsAsync(int salesUserId)
        {
            var followUps = await _repository.GetAllAsync(salesUserId);

            if (!followUps.Any())
                throw new KeyNotFoundException("No follow-ups found for this sales user.");

            return _mapper.Map<IEnumerable<FollowUpResponseDto>>(followUps);
        }

        public async Task<FollowUpResponseDto> GetFollowUpByIdAsync(int followUpId, int salesUserId)
        {
            var followUp = await _repository.GetByIdAsync(followUpId, salesUserId);

            if (followUp == null)
                throw new KeyNotFoundException($"Follow-up with ID {followUpId} not found.");

            return _mapper.Map<FollowUpResponseDto>(followUp);
        }

        public async Task<FollowUpResponseDto> AddFollowUpAsync(CreateFollowUpRequestDto dto, int salesUserId)
        {
            var followUp = new FollowUp(
                dto.LeadId,
                dto.FollowUpDate,
                dto.Remark,
                salesUserId
            );

            await _repository.AddAsync(followUp);

            return _mapper.Map<FollowUpResponseDto>(followUp);
        }

        public async Task<FollowUpResponseDto> UpdateFollowUpAsync(int followUpId, UpdateFollowUpRequestDto dto, int salesUserId)
        {
            var existing = await _repository.GetByIdAsync(followUpId, salesUserId);

            if (existing == null)
                throw new KeyNotFoundException($"Follow-up with ID {followUpId} not found.");

            existing.UpdateFollowUp(dto.FollowUpDate, dto.Remark, salesUserId);

            await _repository.UpdateAsync(existing);

            return _mapper.Map<FollowUpResponseDto>(existing);
        }

        public async Task DeleteFollowUpAsync(int followUpId, int salesUserId)
        {
            var existing = await _repository.GetByIdAsync(followUpId, salesUserId);

            if (existing == null)
                throw new KeyNotFoundException($"Follow-up with ID {followUpId} not found.");

            await _repository.DeleteAsync(existing);
        }
    }
}