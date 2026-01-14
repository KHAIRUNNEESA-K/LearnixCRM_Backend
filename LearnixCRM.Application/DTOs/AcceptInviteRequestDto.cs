using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs
{
    public class AcceptInviteRequestDto
    {
        public string Token { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
    }
}
