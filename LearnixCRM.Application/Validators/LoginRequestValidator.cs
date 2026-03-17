using FluentValidation;
using LearnixCRM.Application.DTOs.User;

namespace LearnixCRM.Application.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .Must(email => !string.IsNullOrWhiteSpace(email))
                .WithMessage("Email cannot be empty or whitespace")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .Must(password => !string.IsNullOrWhiteSpace(password))
                .WithMessage("Password cannot be empty or whitespace")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
                .MaximumLength(50).WithMessage("Password cannot exceed 50 characters")
                .Matches(@"^\S+$").WithMessage("Password cannot contain spaces");
        }
    }
}

