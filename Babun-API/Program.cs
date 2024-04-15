using System.Reflection;
using System.Text;
using Asp.Versioning;
using Babun_API;
using Babun_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using DataInitializer = Babun_API.Data.DataInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddJsonFile("apiAppsettings.json", false, true);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApiContext>(opt => opt.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Version = "v1",
            Title = "Babun Bank News API",
            Description =
                "An api for reading and creating news."
                + "\nAny news created here can be viewed at https://babunbank.azurewebsites.net/"
                + "\nPlease do not type antything racist.",
            TermsOfService = new Uri("https://example.com/contact"),
            Contact = new OpenApiContact
            {
                Name = "Example contact",
                Url = new Uri("https://example.com/contact"),
                Email = ""
            },
            License = new OpenApiLicense
            {
                Name = "",
                Url = new Uri("https://example.com/license"),
            }
        }
    );

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description =
                "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        }
    );

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        }
    );

    options.SwaggerDoc(
        "v2",
        new OpenApiInfo
        {
            Version = "v2",
            Title = "Babun Bank News API - v2",
            Description =
                "An api for reading and creating news. Version 2! To access this api you need to use the Babun Bank Website."
                + "\nThe token is hidden and the token generator will not work for this version of the api."
                + "\nhttps://babunbank.azurewebsites.net/ to see the results.",
            TermsOfService = new Uri("https://example.com/contact"),
            Contact = new OpenApiContact
            {
                Name = "Example contact",
                Url = new Uri("https://example.com/contact"),
                Email = ""
            },
            License = new OpenApiLicense { Name = "", Url = new Uri("https://example.com/contact") }
        }
    );

    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    options.CustomSchemaIds(x => x.Name);

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder
    .Services.AddAuthentication("V1Scheme")
    .AddJwtBearer(
        "V1Scheme",
        x =>
        {
            var settings = builder.Configuration.GetSection("JwtSettings");
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = settings["Issuer"],
                ValidAudience = settings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(settings["Key"]!)
                ),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true
            };
        }
    )
    .AddJwtBearer(
        "V2Scheme",
        options =>
        {
            options.RequireHttpsMetadata = false;
            var settings = builder.Configuration.GetSection("JwtSettings:V2");
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = settings["Issuer"],
                ValidAudience = settings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(settings["Key"]!)
                ),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                RoleClaimType = "V2Claim"
            };
        }
    );

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "V2Policy",
        policy =>
        {
            policy.RequireClaim("V2Claim");
        }
    );
});

builder.Services.AddControllers();

builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ReportApiVersions = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<DataInitializer>();

var app = builder.Build();

Task.Run(async () =>
    {
        await using var scope = app.Services.CreateAsyncScope();
        await scope.ServiceProvider.GetService<DataInitializer>().SeedData();
    })
    .Wait();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint($"/swagger/v1/swagger.json", "V1.0");
    options.SwaggerEndpoint($"/swagger/v2/swagger.json", "V2.0");

    options.OAuthClientId("swagger");
    options.OAuthClientSecret("swagger_secret");
    options.OAuthAppName("Swagger UI");
    options.OAuthUsePkce();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
