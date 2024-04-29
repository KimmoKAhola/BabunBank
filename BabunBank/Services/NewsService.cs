using BabunBank.Configurations.HttpClients;
using BabunBank.Models.FormModels.AdModels;
using BabunBank.Models.ViewModels.ApiBlog;

namespace BabunBank.Services;

public class NewsService
{
    private readonly INewsHttpClient<BlogPost> _newsHttpClient;

    public NewsService(INewsHttpClient<BlogPost> newsHttpClient)
    {
        _newsHttpClient = newsHttpClient;
    }

    public async Task<BlogPost?> Get(int id)
    {
        return await _newsHttpClient.Get(id);
    }

    public async Task<(IEnumerable<BlogPost>?, int?)> GetAll(int pageNumber, int pageSize)
    {
        return await _newsHttpClient.Get(pageNumber, pageSize);
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
