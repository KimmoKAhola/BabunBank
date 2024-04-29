using System.ComponentModel.DataAnnotations;

namespace BabunBank.Infrastructure.Configurations.CustomAnnotations;

public class MaximumTransferAmount(string comparisonProperty) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var comparisonPropertyInfo = validationContext.ObjectType.GetProperty(comparisonProperty);

        if (comparisonPropertyInfo == null || value == null)
        {
            return new ValidationResult($"Property {comparisonProperty} not found.");
        }

        var comparisonValue = (decimal?)
            comparisonPropertyInfo.GetValue(validationContext.ObjectInstance);

        if (comparisonValue == null)
            return new ValidationResult("The value is not a decimal");

        if (value is not decimal decimalValue)
            return new ValidationResult("The value is not a decimal");

        return decimalValue > comparisonValue
            ? new ValidationResult("Amount can not exceed the available balance.")
            : ValidationResult.Success;
    }
}
