using FluentValidation;
using LearnixCRM.Application.DTOs.User;
using LearnixCRM.Domain.Enum;

namespace LearnixCRM.Application.Validators
{
    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequestDto>
    {
        public RegisterUserRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("First name cannot be empty or whitespace")
                .Length(2, 50).WithMessage("First name must be between 2 and 50 characters")
                .Matches("^[a-zA-Z]+$").WithMessage("First name must contain only letters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Last name cannot be empty or whitespace")
                .Length(1, 50).WithMessage("Last name must be between 1 and 50 characters")
                .Matches("^[a-zA-Z]+$").WithMessage("Last name must contain only letters");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required")
                .Matches(@"^\S+$").WithMessage("Email cannot contain spaces")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

            RuleFor(x => x.ContactNumber)
                .NotEmpty().WithMessage("Contact number is required")
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Contact number cannot be empty or whitespace")
                .Matches(@"^\+?[1-9]\d{9,14}$")
                .WithMessage("Invalid contact number format");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required")
                .Must(role => Enum.IsDefined(typeof(UserRole), role))
                .WithMessage("Invalid role selected");
        }
    }
}