using System.ComponentModel.DataAnnotations;

namespace Babun_API.Models;

/// <summary>
/// A model for creating new ads to the database.
/// </summary>
public class CreateAdModel
{
    /// <summary>
    /// A title for the ad. 5-50 characters.
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Please enter a title 5-50 characters.")]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Author name. 2-50 characters.
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter 2-50 characters.")]
    public string Author { get; set; } = null!;

    /// <summary>
    /// Short description. 5-30 characters.
    /// </summary>
    [Required]
    [StringLength(30, MinimumLength = 5, ErrorMessage = "You can enter 5-30 characters.")]
    public string Description { get; set; } = null!;

    /// <summary>
    /// Content for the ad. 50-2000 characters.
    /// </summary>
    [Required]
    [StringLength(
        2000,
        MinimumLength = 50,
        ErrorMessage = "You can enter at most 2000 characters."
    )]
    public string Content { get; set; } = null!;

    /// <summary>
    /// Date Created.
    /// Can be set to a previous date if you wish.
    /// </summary>
    [Required]
    public DateTime DateCreated { get; init; } = DateTime.Now;
}
