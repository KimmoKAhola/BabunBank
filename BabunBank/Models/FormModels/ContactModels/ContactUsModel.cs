﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BabunBank.Models.FormModels.ContactModels;

public record ContactUsModel
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Bajs")]
    [DisplayName("First Name")]
    public string FirstName { get; init; } = null!;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100, MinimumLength = 7, ErrorMessage = "A minimum of 7 characters please.")]
    [DisplayName("Last Name")]
    public string LastName { get; init; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; init; } = null!;

    [Required(ErrorMessage = "Message is required.")]
    [StringLength(
        500,
        MinimumLength = 20,
        ErrorMessage = "Message must be at least 20 characters."
    )]
    public string Message { get; init; } = null!;
}