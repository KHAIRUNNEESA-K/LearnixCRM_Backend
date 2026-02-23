using LearnixCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces.Repositories
{
    public interface IRegistrationRepository
    {
        Task<bool> IsEmailRegisteredAsync(string email);
        Task<bool> IsContactNumberRegisteredAsync(string contactNumber);
        Task CreateUserAsync(User user);
        Task UpdateAsync(User user);
    }
}
