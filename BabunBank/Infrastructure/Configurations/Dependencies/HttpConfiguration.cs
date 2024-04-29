using BabunBank.Infrastructure.Configurations.HttpClients;
using BabunBank.Infrastructure.Interfaces;
using BabunBank.Models.ViewModels.ApiBlog;
using Microsoft.Extensions.Options;

namespace BabunBank.Infrastructure.Configurations.Dependencies;

public static class HttpConfiguration
{
    public static void Configuration(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<NewsHttpClientEndPoints>();
        services.Configure<HttpClientOptions>(configuration.GetSection("HttpClientOptions"));
        services.AddHttpClient<INewsHttpClient<BlogPost>, NewsHttpClient>(
            (services, options) =>
            {
                var httpClientOptions = services
                    .GetRequiredService<IOptions<HttpClientOptions>>()
                    .Value;
                options.BaseAddress = new Uri(httpClientOptions.BaseAddress);
            }
        );
        services.Configure<NewsHttpClientEndPoints>(configuration.GetSection("ServiceEndPoints"));
    }
}
