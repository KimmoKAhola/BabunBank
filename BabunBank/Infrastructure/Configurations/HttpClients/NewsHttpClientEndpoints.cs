namespace BabunBank.Infrastructure.Configurations.HttpClients;

public record NewsHttpClientEndPoints
{
    public string Login { get; init; } = null!;
    public string Ad { get; init; } = null!;
    public string Ads { get; init; } = null!;
    public string Update { get; init; } = null!;
    public string Delete { get; init; } = null!;
}
