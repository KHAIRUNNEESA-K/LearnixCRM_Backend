using LearnixCRM.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces
{
    public interface IBlacklistRepository
    {
        Task<IEnumerable<Blacklist>> GetAllAsync();
        Task<Blacklist?> GetByIdAsync(int id);
        Task<Blacklist?> GetByEmailOrPhoneAsync(string email, string? phone);
    }
}