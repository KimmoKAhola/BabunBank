using System.Text.Json;
using System.Web.Mvc;
using BabunBank.Models.ViewModels.ApiBlog;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BabunBank.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://babun-api.azurewebsites.net/");
    }

    public async Task<BlogPost?> Get(int id)
    {
        var response = await _httpClient.GetAsync($"api/Ad/Get?id={id}");

        response.EnsureSuccessStatusCode();

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var jsonContent = await response.Content.ReadAsStringAsync();

        var blogPost = JsonSerializer.Deserialize<BlogPost>(jsonContent, options);

        return blogPost;
    }

    public async Task<(IEnumerable<BlogPost>?, int)> GetAll(int pageNumber, int pageSize)
    {
        var response = await _httpClient.GetAsync("api/Ad/GetAll");

        response.EnsureSuccessStatusCode();

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var jsonContent = await response.Content.ReadAsStringAsync();

        var blogPostList = JsonSerializer.Deserialize<List<BlogPost>>(jsonContent, options);

        return (
            blogPostList
                ?.OrderByDescending(x => x.DateCreated)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize),
            blogPostList.Count()
        );
    }
}
