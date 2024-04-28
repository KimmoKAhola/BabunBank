using Babun_API.Infrastructure.Configurations;
using DataInitializer = Babun_API.Data.DataInitializer;

var builder = WebApplication.CreateBuilder(args);

ServiceConfigurations.RegisterServices(builder);

SwaggerConfiguration.AddSwaggerDocumentation(builder);
SwaggerConfiguration.AddSwaggerAuthentication(builder);
SwaggerConfiguration.AddSwaggerAuthorization(builder);

builder.Services.AddControllers().AddNewtonsoftJson();

SwaggerConfiguration.AddSwaggerApiVersioning(builder);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<DataInitializer>();

var app = builder.Build();

Task.Run(async () =>
    {
        await using var scope = app.Services.CreateAsyncScope();
        await scope.ServiceProvider.GetService<DataInitializer>()!.SeedData();
    })
    .Wait();

SwaggerConfiguration.UseSwaggerDocumentation(app);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
