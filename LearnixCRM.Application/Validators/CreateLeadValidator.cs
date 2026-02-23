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

            RuleFor(x => x.CourseInterested)
                .IsInEnum().WithMessage("Invalid course type");

            RuleFor(x => x.Source)
                .IsInEnum().WithMessage("Invalid lead source");
        }
    }
}
