using FluentValidation;
using LearnixCRM.Application.DTOs.User;
using Microsoft.AspNetCore.Http;

namespace LearnixCRM.Application.Validators
{
    public class UpdateProfileValidator : AbstractValidator<UpdateProfileDto>
    {
        public UpdateProfileValidator()
        {

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("First name cannot be empty or whitespace")
                .Length(2, 50)
                .WithMessage("First name must be between 2 and 50 characters")
                .Matches("^[a-zA-Z]+$")
                .WithMessage("First name must contain only letters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Last name cannot be empty or whitespace")
                .Length(1, 50)
                .WithMessage("Last name must be between 1 and 50 characters")
                .Matches("^[a-zA-Z]+$")
                .WithMessage("Last name must contain only letters");

            RuleFor(x => x.ContactNumber)
                .Matches(@"^\+?[1-9]\d{9,14}$")
                .WithMessage("Invalid contact number format")
                .When(x => !string.IsNullOrWhiteSpace(x.ContactNumber));

            RuleFor(x => x.ProfileImage)
                .Must(BeValidImageType)
                .WithMessage("Only .jpg, .jpeg, .png files are allowed")
                .When(x => x.ProfileImage != null);

            RuleFor(x => x.ProfileImage)
                .Must(BeValidFileSize)
                .WithMessage("File size must not exceed 2MB")
                .When(x => x.ProfileImage != null);
        }
        private bool BeValidImageType(IFormFile? file)
        {
            if (file == null) return true;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            return allowedExtensions.Contains(extension);
        }

        private bool BeValidFileSize(IFormFile? file)
        {
            if (file == null) return true;

            return file.Length <= 2 * 1024 * 1024; 
        }
    }
}