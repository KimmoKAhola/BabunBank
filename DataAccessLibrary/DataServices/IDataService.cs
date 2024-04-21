namespace DataAccessLibrary.DataServices;

public interface IDataService<T>
    where T : class
{
    Task<T?> GetAsync(int id);
    IQueryable<T> GetAllAsync(string sortOrder, string sortColumn);
    Task<bool?> CreateAsync(T model);
    Task<bool?> DeleteAsync(T model);
    Task<bool?> UpdateAsync(T model);
}
