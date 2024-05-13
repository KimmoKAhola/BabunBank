using System.ComponentModel.DataAnnotations;
using BabunBank.Models.FormModels.Customer;

namespace BabunBank.Infrastructure.Configurations.CustomAnnotations;

public class CompareFullName : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var model = (DeleteCustomerModel)validationContext.ObjectInstance;
        var fullName = model.FirstName + " " + model.Surname;

        if (fullName != (string)value!)
        {
            return new ValidationResult($"Name does not match the full name format: {fullName}");
        }

        return ValidationResult.Success;
    }
}
