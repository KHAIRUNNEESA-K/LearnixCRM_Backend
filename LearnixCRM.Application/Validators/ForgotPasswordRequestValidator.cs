using FluentValidation;
using LearnixCRM.Application.DTOs.SetPasswordToken;

namespace LearnixCRM.Application.Validators
{
    public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequestDto>
    {
        public ForgotPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Email cannot be empty or whitespace")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100)
                .WithMessage("Email cannot exceed 100 characters");
        }
    }
}