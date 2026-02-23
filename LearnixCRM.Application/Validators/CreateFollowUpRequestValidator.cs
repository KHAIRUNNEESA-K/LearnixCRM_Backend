using FluentValidation;
using LearnixCRM.Application.DTOs.FollowUp;

public class CreateFollowUpRequestValidator
    : AbstractValidator<CreateFollowUpRequestDto>
{
    public CreateFollowUpRequestValidator()
    {
        RuleFor(x => x.LeadId)
            .GreaterThan(0).WithMessage("Lead ID must be greater than 0.");

        RuleFor(x => x.FollowUpDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Follow-up date cannot be in the past.");

        RuleFor(x => x.Remark)
            .NotEmpty().WithMessage("Remark is required.")
            .MaximumLength(500).WithMessage("Remark cannot exceed 500 characters.");
    }
}