namespace BabunBank.Infrastructure.Configurations.HttpClients;

public record HttpClientOptions
{
    public string BaseAddress { get; init; } = null!;
}
