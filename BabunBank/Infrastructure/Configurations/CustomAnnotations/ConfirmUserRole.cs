using System.ComponentModel.DataAnnotations;
using DataAccessLibrary.Data;

namespace BabunBank.Infrastructure.Configurations.CustomAnnotations;

public class ConfirmUserRole : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is > UserRole.Choose)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult("Please choose a valid option");
    }
}
