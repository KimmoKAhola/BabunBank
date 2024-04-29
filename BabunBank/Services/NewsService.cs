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
        return await _newsHttpClient.Update(id, model);
    }

    public async Task<bool> Delete(int id)
    {
        return await _newsHttpClient.Delete(id);
    }
}
