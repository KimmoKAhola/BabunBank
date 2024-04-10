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
    [DisplayName("First Name")]
    public string GivenName { get; init; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Please enter at least 2 characters.")]
    [DisplayName("Last Name")]
    public string SurName { get; init; } = null!;

    [Required(ErrorMessage = "The gender is a required field.")]
    [StringLength(6, MinimumLength = 2, ErrorMessage = "Please enter between 2-6 characters.")]
    [DisplayName("Gender")]
    public string Gender { get; init; } = null!;

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
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Please enter at least 2 characters.")]
    [DisplayName("Country")]
    public string Country { get; init; } = null!;

    [Required]
    [StringLength(2, ErrorMessage = "A country code can only consist of 2 characters.")]
    [DisplayName("Country Code")]
    public string CountryCode { get; init; } = null!;

    // [DisplayName("Date of Birth")]
    // public DateOnly? Birthday { get; init; }
    //
    // [StringLength(20, MinimumLength = 2, ErrorMessage = "Please enter at least 2 characters.")]
    // [DisplayName("National Id")]
    // public string? NationalId { get; init; }

    [RegularExpression(@"^\d{2,}$", ErrorMessage = "Only numbers are allowed.")]
    [StringLength(10, ErrorMessage = "Please enter at least 2 numbers.")]
    [DisplayName("Phone Country Code")]
    public string? TelephoneCountryCode { get; init; }

    [StringLength(25, MinimumLength = 2)]
    [RegularExpression(@"^[\d\s]{2,}$", ErrorMessage = "Only numbers and whitespace are allowed.")]
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
