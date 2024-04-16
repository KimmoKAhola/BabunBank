using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Babun_API.Controllers;

/// <summary>
/// Generates a token that is needed for authorization in this api.
/// </summary>
[Route("[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private static readonly TimeSpan TokenLifeTime = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Get a 10-minute token to access the v1 API.
    /// This will not work for v2.
    /// </summary>
    /// <returns>
    /// A token is returned as a simple string. Lasts for 10 minutes.
    /// </returns>
    [ProducesResponseType(typeof(string), 200)]
    [HttpGet]
    public IActionResult GenerateToken()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("apiAppsettings.json", false, true)
            .Build();
        var section = config.GetSection("JwtSettings");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(section["Key"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = section["Issuer"],
            Audience = section["Audience"],
            Expires = DateTime.UtcNow.Add(TokenLifeTime),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(tokenString);
    }
}
