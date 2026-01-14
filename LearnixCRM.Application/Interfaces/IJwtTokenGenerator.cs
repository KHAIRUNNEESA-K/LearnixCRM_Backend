using LearnixCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateJwtToken(int userId, string email, string fullName, string role);
    }
}
