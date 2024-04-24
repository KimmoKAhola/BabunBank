using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace BabunBank.Models.FormModels.AdModels;

public class EditAdModel
{
    [Required]
    [StringLength(50, MinimumLength = 5)]
    [JsonProperty("title")]
    public string Title { get; init; } = null!;

    [Required]
    [StringLength(50, MinimumLength = 2)]
    [JsonProperty("author")]
    public string Author { get; set; } = null!;

    [Required]
    [StringLength(30, MinimumLength = 5)]
    [JsonProperty("description")]
    public string Description { get; init; } = null!;

    [Required]
    [StringLength(2000, MinimumLength = 50)]
    [JsonProperty("content")]
    public string Content { get; set; } = null!;

    [Required]
    [JsonProperty("isDeleted")]
    public bool IsDeleted { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    [JsonProperty("lastModified")]
    public DateTime LastModified { get; set; }
}
