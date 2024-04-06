using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLibrary.Repositories;

public class IdentityUserRepository(BankAppDataContext dbContext)
    : BaseRepository<IdentityUser>(dbContext) { }
