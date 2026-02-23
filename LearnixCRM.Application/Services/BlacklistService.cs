using LearnixCRM.Application.Interfaces;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Services
{
    public class BlacklistService : IBlacklistService
    {
        private readonly IBlacklistRepository _repository;

        public BlacklistService(IBlacklistRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Blacklist>> GetAllAsync()
        {
            var blacklists = await _repository.GetAllAsync();
            if (!blacklists.Any())
            {
                 throw new KeyNotFoundException("No leads in blacklists.");
            }
            return blacklists;


        }

        public async Task<Blacklist?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Blacklist?> GetByEmailOrPhoneAsync(string email, string? phone)
        {
            return await _repository.GetByEmailOrPhoneAsync(email, phone);
        }
    }
}
