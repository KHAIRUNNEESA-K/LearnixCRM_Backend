using LearnixCRM.Application.Interfaces;
using LearnixCRM.Application.Interfaces.Services;
using LearnixCRM.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
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
            var blacklist = await _repository.GetByIdAsync(id);

            if (blacklist == null)
            {
                throw new KeyNotFoundException($"Blacklist entry with ID {id} not found.");
            }

            return blacklist;
        }

        public async Task<Blacklist?> GetByEmailOrPhoneAsync(string email, string? phone)
        {
            var blacklist = await _repository.GetByEmailOrPhoneAsync(email, phone);

            return blacklist;
        }

        public async Task<bool> ExistsInBlacklistAsync(string? email, string? phone)
        {
            return await _repository.ExistsInBlacklistAsync(email, phone);
        }
    }
}
