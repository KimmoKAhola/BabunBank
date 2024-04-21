using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Models.FormModels.Cashier;

public class EditCustomerModel
{
    [Required]
    [HiddenInput]
    public int CustomerId { get; init; }

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Please enter at least 2 characters.")]
    [RegularExpression(@"^\D{2,}$")]
    [DisplayName("First Name")]
    public string GivenName { get; init; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Please enter at least 2 characters.")]
    [RegularExpression(@"^\D{2,}$")]
    [DisplayName("Last Name")]
    public string SurName { get; init; } = null!;

    [Required]
    [Range(1, 3, ErrorMessage = "Please choose a valid option.")]
    [DisplayName("Gender")]
    public int GenderRole { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Please enter at least 2 characters.")]
    [DisplayName("Street Address")]
    public string StreetAddress { get; init; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Please enter at least 2 characters.")]
    [DisplayName("City")]
    public string City { get; init; } = null!;

    [Required]
    [StringLength(15, MinimumLength = 2, ErrorMessage = "Please enter at least 2 characters.")]
    [DisplayName("Zip Code")]
    public string Zipcode { get; init; } = null!;

    [Required]
    [Range(1, 4, ErrorMessage = "Please choose a valid option")]
    public int CountryValue { get; set; }

    public string CountryCode { get; set; } = null!;

    [RegularExpression(@"^\d{2,}$", ErrorMessage = "Only numbers are allowed.")]
    [StringLength(10, ErrorMessage = "Please enter at least 2 numbers.")]
    [DisplayName("Phone Country Code")]
    public string? TelephoneCountryCode { get; init; }

    [StringLength(25, MinimumLength = 2)]
    // [RegularExpression(@"^[\d\s]{2,}$", ErrorMessage = "Only numbers and whitespace are allowed.")]
    [DisplayName("Phone Number")]
    public string? TelephoneNumber { get; init; }

    [
        StringLength(100, ErrorMessage = "Please enter a valid email"),
        RegularExpression(@"^[a-öA-Ö0-9_.-]{2,}@[a-zA-Z]{2,}\.[a-zA-Z]{2,}$")
    ]
    [DisplayName("Email Address")]
    public string? EmailAddress { get; init; }

    [HiddenInput]
    public string? NationalId { get; init; }

    [HiddenInput]
    public DateOnly? Birthday { get; init; }
}
