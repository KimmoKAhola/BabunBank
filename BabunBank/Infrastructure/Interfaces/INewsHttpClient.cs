using BabunBank.Models.FormModels.Ad;

namespace BabunBank.Infrastructure.Interfaces;

public interface INewsHttpClient<T>
    where T : class
{
    Task<T?> Get(int id);
    Task<(IEnumerable<T>?, int?)> Get(int pageNumber, int pageSize);
    Task<bool> Update(int id, EditAdModel model);
    Task<bool> Delete(int id);
}
