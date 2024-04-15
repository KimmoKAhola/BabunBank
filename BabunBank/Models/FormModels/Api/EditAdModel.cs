using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BabunBank.Models.FormModels.Api;

public class EditAdModel
{
    [Required]
    public int id { get; init; }

    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string title { get; init; } = null!;

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string author { get; set; } = null!;

    [Required]
    [StringLength(30, MinimumLength = 5)]
    public string description { get; init; } = null!;

    [Required]
    [StringLength(2000, MinimumLength = 50)]
    public string content { get; set; } = null!;

    [Required]
    public bool isDeleted { get; set; }

    [JsonIgnore]
    public DateTime lastModified { get; set; }
}
