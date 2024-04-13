using Babun_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Babun_API.Data;

public class ApiContext : DbContext
{
    protected ApiContext() { }

    public ApiContext(DbContextOptions<ApiContext> options)
        : base(options) { }

    public DbSet<Ad> Ads { get; init; }
}
