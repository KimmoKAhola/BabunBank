using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Babun_API.Models;

/// <summary>
/// Represents an advertisement.
/// </summary>
public class Ads
{
    /// <summary>
    /// Represents the unique identifier for an advertisement.
    /// </summary>
    [Key]
    [JsonIgnore]
    public int Id { get; init; }

    /// <summary>
    /// Represents the title of an advertisement.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "Please enter a title 5-100 characters.")]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Represents the author of an advertisement.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Enter 2-100 characters.")]
    public string Author { get; set; } = null!;

    /// <summary>
    /// Represents the description of an advertisement.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "You can enter 5-100 characters.")]
    public string Description { get; set; } = null!;

    /// <summary>
    /// Represents the content of an advertisement.
    /// </summary>
    [Required]
    [StringLength(
        10000,
        MinimumLength = 50,
        ErrorMessage = "You can enter at most 10000 characters."
    )]
    public string Content { get; set; } = null!;

    /// <summary>
    /// Represents whether the advertisement has been deleted or not.
    /// </summary>
    [Required]
    [JsonIgnore]
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Represents the date and time the advertisement was created.
    /// </summary>
    [Required]
    public DateTime DateCreated { get; init; } = DateTime.Now;

    /// <summary>
    /// Represents the date and time when the advertisement was last modified.
    /// </summary>
    public DateTime? LastModified { get; set; }
}
