using System.ComponentModel.DataAnnotations;

namespace BabunBank.Infrastructure.Configurations.CustomAnnotations;

public class MinimumAgeAnnotation : ValidationAttribute
{
    private const int MinimumAge = 18;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not DateOnly birthDay)
            return new ValidationResult("Please enter a valid date format.");

        if (birthDay.AddYears(MinimumAge) <= DateOnly.FromDateTime(DateTime.Now))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult("You have to be at least 18 years old to register.");
    }
}
