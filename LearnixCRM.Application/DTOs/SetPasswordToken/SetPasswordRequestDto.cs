using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Application.DTOs.SetPasswordToken
{
    public class SetPasswordRequestDto
    {
        public string Token { get; set; } = default!;

        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
    }
}
