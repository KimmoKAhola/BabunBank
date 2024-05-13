using Babun_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Babun_API.Data;

/// <summary>
/// Represents the context for the API.
/// </summary>
public class ApiContext : DbContext
{
    /// <inheritdoc />
    protected ApiContext() { }

    /// <inheritdoc />
    public ApiContext(DbContextOptions<ApiContext> options)
        : base(options) { }

    /// <summary>
    /// Represents an advertisement.
    /// </summary>
    public DbSet<Ads> Ads { get; init; }
}
