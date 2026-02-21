using LearnixCRM.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace LearnixCRM.Application.DTOs.User
{
    public class RegisterUserRequestDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string ContactNumber { get; set; } = default!;
        public int Role { get; set; }
    }
}
