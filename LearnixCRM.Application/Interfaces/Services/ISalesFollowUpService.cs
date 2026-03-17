using LearnixCRM.Application.DTOs.FollowUp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Services
{
    public interface ISalesFollowUpService
    {
        Task<IEnumerable<FollowUpResponseDto>> GetAllFollowUpsAsync(int salesUserId);
        Task<FollowUpResponseDto> GetFollowUpByIdAsync(int followUpId, int salesUserId);
        Task<FollowUpResponseDto> AddFollowUpAsync(CreateFollowUpRequestDto dto, int salesUserId);
        Task<FollowUpResponseDto> UpdateFollowUpAsync(int followUpId, UpdateFollowUpRequestDto dto, int salesUserId);
        Task DeleteFollowUpAsync(int followUpId, int salesUserId);
    }
}