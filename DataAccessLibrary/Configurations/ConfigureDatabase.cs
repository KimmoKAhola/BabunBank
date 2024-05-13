using Microsoft.Extensions.Configuration;

namespace DataAccessLibrary.Configurations;

public class ConfigureDatabase(IConfiguration configuration)
{
    public string Configure()
    {
        return configuration.GetConnectionString("DefaultConnection")!;
    }
}
