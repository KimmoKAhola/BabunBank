using System.ComponentModel.DataAnnotations;

namespace BabunBank.Models;

public class CreateCustomerModel
{
    [Required]
    [MaxLength(6)]
    public string Gender { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string GivenName { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string SurName { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string StreetAddress { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string City { get; set; } = null!;

    [Required]
    [StringLength(15, MinimumLength = 2)]
    public string Zipcode { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Country { get; set; } = null!;

    [Required]
    [StringLength(2, MinimumLength = 2)]
    public string CountryCode { get; set; } = null!;

    public DateOnly? Birthday { get; set; }

    [StringLength(20, MinimumLength = 2)]
    public string? NationalId { get; set; }

    [StringLength(10, MinimumLength = 2)]
    public string? TelephoneCountryCode { get; set; }

    [StringLength(25, MinimumLength = 2)]
    public string? TelephoneNumber { get; set; }

    [StringLength(20), RegularExpression(@"^[a-öA-Ö0-9_.-]{2,}@[a-zA-Z]{2,}\.[a-zA-Z]{2,}$")]
    public string? EmailAddress { get; set; }
}
