using System.ComponentModel.DataAnnotations;

namespace BabunBank.Models.Customer;

public class SignUpCustomerModel
{
    [Required(ErrorMessage = "The gender is a required field.")]
    [StringLength(6, MinimumLength = 2, ErrorMessage = "Please enter between 2-6 characters.")]
    public string Gender { get; init; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Please enter at least 2 characters.")]
    public string GivenName { get; init; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Please enter at least 2 characters.")]
    public string SurName { get; init; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Please enter at least 2 characters.")]
    public string StreetAddress { get; init; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Please enter at least 2 characters.")]
    public string City { get; init; } = null!;

    [Required]
    [StringLength(15, MinimumLength = 2, ErrorMessage = "Please enter at least 2 characters.")]
    public string Zipcode { get; init; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Please enter at least 2 characters.")]
    public string Country { get; init; } = null!;

    [Required]
    [StringLength(2, ErrorMessage = "A country code can only consist of 2 characters.")]
    public string CountryCode { get; init; } = null!;

    public DateOnly? Birthday { get; init; }

    [StringLength(20, MinimumLength = 2, ErrorMessage = "Please enter at least 2 characters.")]
    public string? NationalId { get; init; }

    [RegularExpression(@"^\+[0-9]+$", ErrorMessage = "Only numbers are allowed after the + sign.")]
    [StringLength(10, ErrorMessage = "Please enter at least 2 characters, starting with a +.")]
    public string? TelephoneCountryCode { get; init; }

    [StringLength(25, MinimumLength = 2)]
    [RegularExpression(@"^\d{2,}$", ErrorMessage = "Only numbers are allowed.")]
    public string? TelephoneNumber { get; init; }

    [
        StringLength(20, ErrorMessage = "Please enter a valid email"),
        RegularExpression(@"^[a-öA-Ö0-9_.-]{2,}@[a-zA-Z]{2,}\.[a-zA-Z]{2,}$")
    ]
    public string? EmailAddress { get; init; }
}
