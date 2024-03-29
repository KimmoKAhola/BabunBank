using BabunBank.Data;
using Microsoft.AspNetCore.Identity;

namespace BabunBank.Repositories;

public class UserRepository(ApplicationDbContext dbContext) : BaseRepository<IdentityUser>(dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;
}