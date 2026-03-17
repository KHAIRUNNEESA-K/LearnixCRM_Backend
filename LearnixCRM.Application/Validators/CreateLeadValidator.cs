using FluentValidation;
using LearnixCRM.Application.DTOs.Lead;

namespace LearnixCRM.Application.Validators.Lead
{
    public class CreateLeadRequestValidator : AbstractValidator<CreateLeadRequestDto>
    {
        public CreateLeadRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^[0-9]{10}$")
                .WithMessage("Phone number must be 10 digits");

            RuleFor(x => x.CourseId)
                .GreaterThan(0)
                .WithMessage("CourseId must be greater than 0");

            RuleFor(x => x.Source)
                .IsInEnum()
                .WithMessage("Invalid lead source");
        }
    }
}
