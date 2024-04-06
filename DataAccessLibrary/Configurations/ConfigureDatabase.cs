using Microsoft.Extensions.Configuration;

namespace DataAccessLibrary.Configurations;

public class ConfigureDatabase
{
    private readonly IConfiguration _configuration;

    public ConfigureDatabase(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Configure()
    {
        return _configuration.GetConnectionString("DefaultConnection");
    }
}
