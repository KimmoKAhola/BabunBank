using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BabunBank.Models.CustomAnnotations;
using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Models.FormModels.Customer;

public class SignUpCustomerModel
{
    [Required]
    [Range(1, 3)]
    [DisplayName("Gender")]
    public int GenderRole { get; init; }

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Please enter at least 2 characters.")]
    [DisplayName("First Name")]
    public string GivenName { get; init; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Please enter at least 2 characters.")]
    [DisplayName("Last Name")]
    public string SurName { get; init; } = null!;

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
    [Range(1, 5)]
    [DisplayName("Country")]
    public int Country { get; init; }

    [Required]
    [StringLength(2, ErrorMessage = "A country code can only consist of 2 characters.")]
    [DisplayName("Country Code")]
    public string CountryCode { get; init; } = null!;

    [DisplayName("Date of Birth")]
    [MinimumAgeAnnotation]
    public DateOnly? Birthday { get; init; }

    [StringLength(20, MinimumLength = 2, ErrorMessage = "Please enter at least 2 characters.")]
    [DisplayName("National Id")]
    public string? NationalId { get; init; }

    [RegularExpression(@"^\d{2,}$", ErrorMessage = "Only numbers are allowed.")]
    [StringLength(10, ErrorMessage = "Please enter at least 2 numbers.")]
    [DisplayName("Phone Country Code")]
    public string? TelephoneCountryCode { get; init; }

    [StringLength(25, MinimumLength = 2)]
    [RegularExpression(@"^\d{2,}$", ErrorMessage = "Only numbers are allowed.")]
    [DisplayName("Phone Number")]
    public string? TelephoneNumber { get; init; }

    [
        StringLength(100, ErrorMessage = "Please enter a valid email"),
        RegularExpression(@"^[a-öA-Ö0-9_.-]{2,}@[a-zA-Z]{2,}\.[a-zA-Z]{2,}$")
    ]
    [DisplayName("Email Address")]
    [EmailAddress]
    public string? EmailAddress { get; init; }

    [DisplayName("Confirm Email")]
    [EmailAddress]
    [Compare("EmailAddress", ErrorMessage = "Email and confirm email must match.")]
    public string? ConfirmEmail { get; init; }
}
