using FluentValidation;
using LearnixCRM.Application.DTOs.Lead;

namespace LearnixCRM.Application.Validators.Lead
{
    public class UpdateLeadRequestValidator : AbstractValidator<UpdateLeadRequestDto>
    {
        public UpdateLeadRequestValidator()
        {
            RuleFor(x => x.LeadId)
                .GreaterThan(0).WithMessage("Lead ID must be greater than zero");

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