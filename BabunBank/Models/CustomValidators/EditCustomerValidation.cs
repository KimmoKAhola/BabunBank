using BabunBank.Models.FormModels.Cashier;
using FluentValidation;

namespace BabunBank.Models.CustomValidators;

public class EditCustomerValidation : AbstractValidator<EditCustomerModel>
{
    public EditCustomerValidation()
    {
        RuleFor(customer => customer.CustomerId).NotNull();

        RuleFor(customer => customer.GivenName)
            .NotEmpty()
            .WithMessage("Field can not be empty.")
            .Matches(@"^\D{2,}$")
            .WithMessage("No numbers are allowed.")
            .Length(2, 100)
            .WithMessage("The name has to be between 2-100 characters long.");

        RuleFor(customer => customer.SurName)
            .NotEmpty()
            .WithMessage("Field can not be empty.")
            .Matches(@"^\D{2,}$")
            .WithMessage("No numbers are allowed.")
            .Length(2, 100)
            .WithMessage("The name has to be between 2-100 characters long.");

        RuleFor(customer => customer.GenderRole)
            .InclusiveBetween(1, 3)
            .WithMessage("Please choose a valid option.");

        RuleFor(customer => customer.StreetAddress);

        RuleFor(customer => customer.City);

        RuleFor(customer => customer.Zipcode);

        RuleFor(customer => customer.CountryValue)
            .InclusiveBetween(1, 4)
            .WithMessage("Please choose a valid option");

        RuleFor(customer => customer.TelephoneNumber)
            .NotNull()
            .Matches(@"^(?:(?=\-)|\d)[-\d\s]*$")
            .WithMessage("Only digits and '-' are allowed.");

        RuleFor(customer => customer.TelephoneCountryCode);

        RuleFor(customer => customer.EmailAddress)
            .NotEmpty()
            .WithMessage("Field can not be empty.")
            .EmailAddress()
            .WithMessage("Please provide a valid address");
    }
}
