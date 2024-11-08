using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Asp.Versioning;
using Babun_API.Infrastructure.Configurations;
using Babun_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Babun_API.Controllers;

/// <summary>
/// This class generates a token that is needed for authorization in the API.
/// </summary>
[Route("/[action]")]
[ApiController]
public class TokenController(
    SignInManager<IdentityUser> signInManager,
    UserManager<IdentityUser> userManager
) : ControllerBase
{
    private static readonly TimeSpan TokenLifeTime = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Get a 10-minute token to access the v1 API.
    /// This will not work for v2.
    /// </summary>
    /// <returns>
    /// A token is returned as a simple string. Lasts for 10 minutes.
    /// </returns>
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ProducesResponseType(typeof(string), 200)]
    [HttpGet]
    public IActionResult GenerateToken()
    {
        var tokenString = GenerateTokenString(SwaggerConfiguration.V1ClaimType);

        return Ok(tokenString);
    }

    private static string GenerateTokenString(string claimType)
    {
        var section = LoadJwtSettings();

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(section["Key"]!);

        var tokenDescriptor = SecurityTokenDescriptor(section, key, claimType);

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        return tokenString;
    }

    private static SecurityTokenDescriptor SecurityTokenDescriptor(
        IConfiguration section,
        byte[] key,
        string claimType
    )
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = section["Issuer"],
            Audience = section["Audience"],
            Expires = DateTime.UtcNow.Add(TokenLifeTime),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
            Subject = new ClaimsIdentity(new[] { new Claim(claimType, "true") })
        };
        return tokenDescriptor;
    }

    private static IConfigurationSection LoadJwtSettings()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("apiAppsettings.json", false, true)
            .Build();
        var section = config.GetSection("JwtSettings:V2");
        return section;
    }

    /// <summary>
    /// Logs in a user and generates a token for authorization in the API.
    /// </summary>
    /// <param name="login">The login information of the user.</param>
    /// <returns>
    /// If the login is successful, a token is returned as a simple string.
    /// </returns>
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName = "v2")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginModel login)
    {
        try
        {
            var userExists = await userManager.FindByEmailAsync(login.Username);

            if (userExists == null)
            {
                return Unauthorized();
            }

            var signInResult = await signInManager.CheckPasswordSignInAsync(
                userExists,
                login.Password,
                lockoutOnFailure: false
            );

            if (!signInResult.Succeeded)
            {
                return Unauthorized();
            }

            var token = GenerateTokenString(SwaggerConfiguration.V2ClaimType);

            return Ok(token);
        }
        catch (Exception e)
        {
            return Ok(e + "\n" + e.Message + "\n" + e.InnerException);
        }
    }
}
