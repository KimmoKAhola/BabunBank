using BabunBank.Models.FormModels.Home;
using FluentValidation;

namespace BabunBank.Models.CustomValidators;

public class ContactUsValidator : AbstractValidator<ContactUsModel>
{
    public ContactUsValidator()
    {
        RuleFor(user => user.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .Matches(@"^\D*$")
            .WithMessage("First name can not contain any numbers.")
            .Length(2, 100)
            .WithMessage("Please enter between 2 and 100 characters.");

        RuleFor(user => user.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .NotNull()
            .WithMessage("Last name is required.")
            .Matches(@"^\D*$")
            .WithMessage("Last name can not contain any numbers.")
            .Length(2, 100)
            .WithMessage("Please enter between 2 and 100 characters");

        RuleFor(user => user.Email).EmailAddress();

        RuleFor(user => user.Message)
            .NotEmpty()
            .WithMessage("This field can not be left empty")
            .MinimumLength(25)
            .WithMessage("Please submit at least 20 characters")
            .WithName("Your Text Meessagea");
    }
}
