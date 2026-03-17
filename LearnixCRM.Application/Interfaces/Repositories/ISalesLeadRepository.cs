using LearnixCRM.Application.DTOs.Lead;
using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces
{
    public interface ISalesLeadRepository
    {
        
        Task<Lead?> GetByIdAsync(int leadId);

        Task<IEnumerable<LeadResponseDto>> GetAllAsync();

        Task AddAsync(Lead lead);

        Task UpdateAsync(Lead lead);

        Task DeleteAsync(Lead lead);

        Task<IEnumerable<Lead>> GetByStatusAsync(LeadStatus status);
        Task AddBlacklistAsync(Blacklist blacklist);
        Task AddStudentAsync(Student student);
        Task<List<string>> GetAllEmailsAsync();

        Task<bool> ExistsByEmailOrPhoneAsync(string email, string phone);
    }
}
