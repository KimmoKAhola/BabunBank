namespace Babun_API.Models;

/// <summary>
/// A model for displaying database objects.
/// All properties are immutable.
/// </summary>
public class ViewAdModel
{
    /// <summary>
    /// The database id for the ad.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Title for the ad.
    /// </summary>
    public string Title { get; init; } = null!;

    /// <summary>
    /// The name of the Author.
    /// </summary>
    public string Author { get; init; } = null!;

    /// <summary>
    /// The ad's description.
    /// </summary>
    public string Description { get; init; } = null!;

    /// <summary>
    /// The ad's content.
    /// The text body.
    /// </summary>
    public string Content { get; init; } = null!;

    /// <summary>
    /// Property for soft deletion.
    /// </summary>
    public bool IsDeleted { get; init; }

    /// <summary>
    /// Display when the ad was created.
    /// </summary>
    public DateTime DateCreated { get; init; }

    /// <summary>
    /// Property to display if/when the ad was modified.
    /// Nullable.
    /// </summary>
    public DateTime? LastModified { get; init; }
}
