using BabunBank.Data;

namespace BabunBank.Repositories;

public class UserRepository(ApplicationDbContext dbContext) : BaseRepository<User>(dbContext)
{
    private readonly ApplicationDbContext _dbContext = dbContext;
}