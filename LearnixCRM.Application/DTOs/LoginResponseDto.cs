namespace LearnixCRM.Application.DTOs
{
    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Role { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}
