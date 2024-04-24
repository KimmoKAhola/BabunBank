using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using BabunBank.Models.FormModels.AdModels;
using BabunBank.Models.FormModels.User;
using BabunBank.Models.ViewModels.ApiBlog;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BabunBank.Services;

public class NewsService
{
    private readonly HttpClient _httpClient;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public NewsService(
        IHttpClientFactory httpClientFactory,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager
    )
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://babun-api.azurewebsites.net/");
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<BlogPost?> Get(int id)
    {
        var response = await _httpClient.GetAsync($"/Ads/<id>?id={id}");

        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var jsonContent = await response.Content.ReadAsStringAsync();

        var blogPost = JsonSerializer.Deserialize<BlogPost>(jsonContent, options);

        return blogPost;
    }

    public async Task<(IEnumerable<BlogPost>?, int?)> GetAll(int pageNumber, int pageSize)
    {
        var response = await _httpClient.GetAsync("/Ads");

        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            return (null, null);
        }

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var jsonContent = await response.Content.ReadAsStringAsync();

        var blogPostList = JsonSerializer.Deserialize<List<BlogPost>>(jsonContent, options);

        return (
            blogPostList
                ?.OrderByDescending(x => x.DateCreated)
                .Where(x => !x.IsDeleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize),
            blogPostList.Count()
        );
    }

    public async Task<string> Test(User user)
    {
        var existingUser = await _userManager.FindByEmailAsync(user.UserName);

        if (existingUser == null)
            return null;

        var result = await _signInManager.CheckPasswordSignInAsync(
            existingUser,
            user.Password,
            lockoutOnFailure: false
        );

        if (!result.Succeeded)
        {
            return null;
        }

        if (result.Succeeded)
        {
            var token = GenerateToken();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );

            var response = await _httpClient.GetAsync("api/v2/Ad");

            var test = await response.Content.ReadAsStringAsync();
            return test;
        }

        return null;
    }

    private async Task<string> ValidateUser(string username, string password)
    {
        var requestBody = new { username, password };

        var requestBodyJson = JsonSerializer.Serialize(requestBody);
        var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

        var tokenResponse = await _httpClient.PostAsync("/Login", requestContent);

        return await tokenResponse.Content.ReadAsStringAsync();
    }

    private string GenerateToken()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        var section = configuration.GetSection("JwtSettings:V2");

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = section["Issuer"],
            Audience = section["Audience"],
            Expires = DateTime.UtcNow.AddHours(15),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(section["Key"]!)),
                SecurityAlgorithms.HmacSha256Signature
            ),
            Subject = new ClaimsIdentity(
                new[]
                {
                    new Claim("V2Claim", "true") // Add the desired claim here
                }
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor); //fel här?
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }

    public async Task<bool> Update(int id, EditAdModel model)
    {
        try
        {
            var token = await ValidateUser("richard.chalk@systementor.se", "Hejsan123#");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );

            var jsonModel = JsonSerializer.Serialize(model);
            var modelContent = new StringContent(jsonModel, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/v2/Ad/{id}", modelContent);

            response.EnsureSuccessStatusCode();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> Delete(int id)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var response = await _httpClient.DeleteAsync($"/Ads/{id}");

        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }
}
