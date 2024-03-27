
using BabunBank.Data;

namespace BabunBank.Repositories;

public class CustomerRepository(ApplicationDbContext dbContext)
    : BaseRepository<Customer>(dbContext) { }