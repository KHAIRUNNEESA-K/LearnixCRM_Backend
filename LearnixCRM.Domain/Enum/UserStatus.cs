using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Domain.Enum
{
    public enum UserStatus
    {
        Inactive = 0,
        Pending = 1,
        Approved = 2,
        Active = 3,
        Rejected = 4,
        Blocked=5
    }
}
