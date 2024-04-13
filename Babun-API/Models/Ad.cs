using System.ComponentModel.DataAnnotations;

namespace Babun_API.Models;

public class Ad
{
    [Key]
    [Required]
    public int Id { get; init; }

    [Required]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Please enter a title 5-50 characters.")]
    public string Title { get; init; } = null!;

    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Enter 2-50 characters.")]
    public string Author { get; set; } = null!;

    [Required]
    [StringLength(30, MinimumLength = 5, ErrorMessage = "You can enter 5-30 characters.")]
    public string Description { get; init; } = null!;

    [Required]
    [StringLength(
        2000,
        MinimumLength = 50,
        ErrorMessage = "You can enter at most 2000 characters."
    )]
    public string Content { get; set; } = null!;

    [Required]
    public DateTime DateCreated { get; set; } = DateTime.Now;

    public DateTime? LastModified { get; set; }
}
