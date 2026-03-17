using FluentValidation;
using LearnixCRM.Application.DTOs.Course;

namespace LearnixCRM.Application.Validators.Course
{
    public class CreateCourseDtoValidator : AbstractValidator<CreateCourseDto>
    {
        public CreateCourseDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Course name is required")
                .MaximumLength(100).WithMessage("Course name cannot exceed 100 characters");
            RuleFor(x => x.CourseDuration)
                .GreaterThan(0).WithMessage("Course duration must be greater than zero")
                .LessThanOrEqualTo(24).WithMessage("Duration cannot exceed 24 months");
            RuleFor(x => x.Fee)
                .GreaterThan(0).WithMessage("Course fee must be greater than zero")
                .LessThanOrEqualTo(1000000)
                .WithMessage("Course fee is too large");
        }
    }
}