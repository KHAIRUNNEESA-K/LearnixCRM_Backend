using FluentValidation;
using LearnixCRM.Application.DTOs.Lead;

namespace LearnixCRM.Application.Validators.Lead
{
    public class UpdateLeadRequestValidator : AbstractValidator<UpdateLeadRequestDto>
    {
        public UpdateLeadRequestValidator()
        {
            RuleFor(x => x.LeadId)
                .GreaterThan(0)
                .WithMessage("Lead ID must be greater than zero");

            RuleFor(x => x.FullName)
                .MaximumLength(100)
                .WithMessage("Full name cannot exceed 100 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.FullName));

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Invalid email format")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.Phone)
                .Matches(@"^[0-9]{10}$")
                .WithMessage("Phone number must be 10 digits")
                .When(x => !string.IsNullOrWhiteSpace(x.Phone));

            RuleFor(x => x.CourseId)
                .GreaterThan(0)
                .WithMessage("CourseId must be greater than 0")
                .When(x => x.CourseId.HasValue);

            RuleFor(x => x.Source)
                .IsInEnum()
                .WithMessage("Invalid lead source")
                .When(x => x.Source.HasValue);

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid lead status")
                .When(x => x.Status.HasValue);
        }
    }
}

