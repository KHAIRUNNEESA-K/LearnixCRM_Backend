using System.ComponentModel.DataAnnotations;

namespace LearnixCRM.Application.DTOs.User
{
    public class LoginRequestDto
    {
        public string Email { get; set; } = default!;

        public string Password { get; set; } = default!;
    }
}
