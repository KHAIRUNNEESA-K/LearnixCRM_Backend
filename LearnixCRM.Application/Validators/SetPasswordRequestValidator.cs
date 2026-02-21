using FluentValidation;
using LearnixCRM.Application.DTOs.SetPasswordToken;

namespace LearnixCRM.Application.Validators
{
    public class SetPasswordRequestValidator : AbstractValidator<SetPasswordRequestDto>
    {
        public SetPasswordRequestValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required")
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Token cannot be empty or whitespace");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Password cannot be empty or whitespace")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required")
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match");
        }
    }
}