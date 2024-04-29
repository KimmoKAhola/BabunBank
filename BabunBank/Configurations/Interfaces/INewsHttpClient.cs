using BabunBank.Models.FormModels.AdModels;

namespace BabunBank.Configurations.Interfaces;

public interface INewsHttpClient<T>
    where T : class
{
    Task<T?> Get(int id);
    Task<(IEnumerable<T>?, int?)> Get(int pageNumber, int pageSize);
    Task<bool> Update(int id, EditAdModel model);
    Task<bool> Delete(int id);
}
