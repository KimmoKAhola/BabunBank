namespace DataAccessLibrary.DataServices;

public interface IDataService<T> where T : class
{
    Task<T> GetAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
}