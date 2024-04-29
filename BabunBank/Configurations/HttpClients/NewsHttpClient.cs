using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BabunBank.Configurations.Interfaces;
using BabunBank.Models.FormModels.AdModels;
using BabunBank.Models.ViewModels.ApiBlog;
using Microsoft.Extensions.Options;

namespace BabunBank.Configurations.HttpClients;

//TODO check if this fits better as NewsService instead
public class NewsHttpClient : INewsHttpClient<BlogPost>
{
    private readonly HttpClient _httpClient;
    private readonly NewsHttpClientEndPoints _httpEndPoints;
    private readonly JsonSerializerOptions _jsonOptions =
        new() { PropertyNameCaseInsensitive = true };

    public NewsHttpClient(HttpClient httpClient, IOptions<NewsHttpClientEndPoints> httpEndPoints)
    {
        _httpClient = httpClient;
        _httpEndPoints = httpEndPoints.Value;
    }

    public async Task<BlogPost?> Get(int id)
    {
        var httpResponseMessage = await _httpClient.GetAsync($"{_httpEndPoints.Ad}{id}");
        try
        {
            httpResponseMessage.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }

        var jsonContent = await httpResponseMessage.Content.ReadAsStringAsync();
        var blogpost = JsonSerializer.Deserialize<BlogPost>(jsonContent, _jsonOptions);
        return blogpost;
    }

    public async Task<(IEnumerable<BlogPost>?, int?)> Get(int pageNumber, int pageSize)
    {
        var httpResponseMessage = await _httpClient.GetAsync(_httpEndPoints.Ads);
        try
        {
            httpResponseMessage.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        var jsonContent = await httpResponseMessage.Content.ReadAsStringAsync();
        var blogPosts = JsonSerializer.Deserialize<List<BlogPost>>(jsonContent, _jsonOptions);
        return (
            blogPosts
                .OrderByDescending(x => x.DateCreated)
                .Where(x => !x.IsDeleted)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize),
            blogPosts.Count
        );
    }

    public async Task<bool> Update(int id, EditAdModel model)
    {
        try
        {
            //Note, this is hard-coded only for demonstration purposes and should NOT be used in production!!
            //It is only left here to demonstrate authorization and authentication using JWT.
            var token = await ValidateUser("richard.chalk@systementor.se", "Hejsan123#");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );

            var jsonModel = JsonSerializer.Serialize(model);
            var modelContent = new StringContent(jsonModel, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_httpEndPoints.Update}{id}", modelContent);

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
        var response = await _httpClient.DeleteAsync($"{_httpEndPoints.Delete}{id}");

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

    private async Task<string> ValidateUser(string username, string password)
    {
        var requestBody = new { username, password };

        var requestBodyJson = JsonSerializer.Serialize(requestBody);
        var requestContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

        var tokenResponse = await _httpClient.PostAsync(_httpEndPoints.Login, requestContent);

        return await tokenResponse.Content.ReadAsStringAsync();
    }
}
