using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs
{
    public class UserInviteDto
    {
        public int InviteId { get; set; }
        public string Email { get; set; } = null!;
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
