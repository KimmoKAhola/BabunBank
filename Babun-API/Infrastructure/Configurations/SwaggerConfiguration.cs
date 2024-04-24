using System.Reflection;
using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Babun_API.Infrastructure.Configurations;

/// <summary>
/// Configuration class for Swagger documentation.
/// </summary>
public static class SwaggerConfiguration
{
    /// <summary>
    /// The version number of the API.
    /// </summary>
    public const string Version1 = "v1";

    /// <summary>
    /// The second version number of the API.
    /// </summary>
    public const string Version2 = "v2";

    /// <summary>
    /// Represents the authentication scheme used for API version 1.
    /// </summary>
    public const string V1Scheme = "V1Scheme";

    /// <summary>
    /// V2Scheme represents the authentication scheme used in version 2 of the API.
    /// </summary>
    public const string V2Scheme = "V2Scheme";

    /// <summary>
    /// The policy name for API version 2.0 authentication.
    /// </summary>
    public const string V2Policy = "V2Policy";
    public const string V1ClaimType = "V1Claim";
    public const string V2ClaimType = "V2Claim";
    private const string Contact = "https://example.com/contact";
    private const string License = "https://example.com/license";
    private const string Email = "example@example.example";
    private const string ExampleName = "Example Name";
    private const string JwtSettings = "JwtSettings";
    private const string JwtSettingsV2 = "JwtSettings:V2";
    private const string MyWebsite = "https://babunbank.azurewebsites.net/";

    /// <summary>
    /// Adds Swagger documentation to the Web Application.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance.</param>
    public static void AddSwaggerDocumentation(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                Version1,
                new OpenApiInfo
                {
                    Version = Version1,
                    Title = "Babun Bank News API",
                    Description =
                        $"An api for reading and creating news."
                        + $"\nAny news created here can be viewed at {MyWebsite}"
                        + "\nPlease do not type anything racist.",
                    TermsOfService = new Uri(Contact),
                    Contact = new OpenApiContact
                    {
                        Name = ExampleName,
                        Url = new Uri(Contact),
                        Email = Email
                    },
                    License = new OpenApiLicense { Name = "", Url = new Uri(License) }
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
                Version2,
                new OpenApiInfo
                {
                    Version = Version2,
                    Title = "Babun Bank News API - v2",
                    Description =
                        $"An api for reading and creating news. Version 2! To access this api you need to use the Babun Bank Website."
                        + "\nThe token is hidden and the token generator will not work for this version of the api."
                        + $"\n{MyWebsite} to see the results.",
                    TermsOfService = new Uri(Contact),
                    Contact = new OpenApiContact
                    {
                        Name = ExampleName,
                        Url = new Uri(Contact),
                        Email = Email
                    },
                    License = new OpenApiLicense { Name = ExampleName, Url = new Uri(License) }
                }
            );

            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            options.CustomSchemaIds(x => x.Name);

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }

    /// <summary>
    /// Adds Swagger authentication to the Web Application.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance.</param>
    public static void AddSwaggerAuthentication(WebApplicationBuilder builder)
    {
        builder
            .Services.AddAuthentication(V1Scheme)
            .AddJwtBearer(
                V1Scheme,
                x =>
                {
                    var settings = builder.Configuration.GetSection(JwtSettings);
                    ConfigureTokenValidationParameters(x, settings, V1ClaimType);
                }
            )
            .AddJwtBearer(
                V2Scheme,
                options =>
                {
                    options.RequireHttpsMetadata = false;
                    var settings = builder.Configuration.GetSection(JwtSettingsV2);
                    options.SaveToken = true;
                    ConfigureTokenValidationParameters(options, settings, V2ClaimType);
                }
            );
    }

    private static void ConfigureTokenValidationParameters(
        JwtBearerOptions jwtBearerOptions,
        IConfigurationSection settings,
        string? roleClaimType
    )
    {
        jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = settings["Issuer"],
            ValidAudience = settings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings["Key"]!)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = roleClaimType
        };
    }

    /// <summary>
    /// Adds Swagger authorization to the Web Application.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance.</param>
    public static void AddSwaggerAuthorization(WebApplicationBuilder builder)
    {
        builder
            .Services.AddAuthorizationBuilder()
            .AddPolicy(
                V2Policy,
                policy =>
                {
                    policy.RequireClaim(V2ClaimType);
                }
            );
    }

    /// <summary>
    /// Adds Swagger documentation to the Web Application.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
    public static void UseSwaggerDocumentation(IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Babun Bank News API v1");
            options.SwaggerEndpoint("/swagger/v2/swagger.json", "Babun Bank News API v2");
            options.OAuthClientId("swagger");
            options.OAuthClientSecret("swagger_secret");
            options.OAuthAppName("Swagger UI");
            options.OAuthUsePkce();
        });
    }

    /// <summary>
    /// Adds API versioning to the Swagger documentation in the Web Application.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance.</param>
    public static void AddSwaggerApiVersioning(WebApplicationBuilder builder)
    {
        builder.Services.AddApiVersioning(o =>
        {
            o.DefaultApiVersion = new ApiVersion(1, 0);
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.ReportApiVersions = true;
        });
    }
}
