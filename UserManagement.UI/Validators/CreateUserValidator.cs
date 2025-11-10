using FluentValidation;
using UserManagement.UI.DTOs;

namespace UserManagement.UI.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserRequestModel>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Forename)
            .NotEmpty().WithMessage("Forename is required.")
            .MaximumLength(50).WithMessage("Forename cannot exceed 50 characters.");

        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("Surname is required.")
            .MaximumLength(50).WithMessage("Surname cannot exceed 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now).WithMessage("Date of Birth must be in the past.");
    }
}
