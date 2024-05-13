using System.ComponentModel.DataAnnotations;

namespace BabunBank.Infrastructure.Configurations.CustomAnnotations;

public class DateTransactionCheck : ValidationAttribute
{
    private static DateOnly Today => DateOnly.FromDateTime(DateTime.Now);

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not DateOnly)
        {
            return new ValidationResult("Please enter a valid date format");
        }

        if (value is DateOnly dateOnly && dateOnly >= Today)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult("You can not enter a transaction date in the past.");
    }
}
