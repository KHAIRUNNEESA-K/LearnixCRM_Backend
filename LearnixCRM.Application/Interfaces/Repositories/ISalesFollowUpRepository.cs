using LearnixCRM.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces
{
    public interface ISalesFollowUpRepository
    {
        Task<IEnumerable<FollowUp>> GetAllAsync(int salesUserId);
        Task<FollowUp?> GetByIdAsync(int followUpId, int salesUserId);
        Task AddAsync(FollowUp followUp);
        Task UpdateAsync(FollowUp followUp);
        Task DeleteAsync(FollowUp followUp);
    }
}