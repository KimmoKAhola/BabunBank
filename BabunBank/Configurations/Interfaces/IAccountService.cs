using BabunBank.Models.ViewModels.Account;
using DataAccessLibrary.Data;

namespace BabunBank.Configurations.Interfaces;

public interface IAccountService
{
    Task<AccountViewModel?> GetAccountViewModelAsync(int id);
    Task<IEnumerable<Account>> RenameMe(int id, int pageNumber, int pageSize, string q);
}
