using DataAccessLibrary.Data;
using DetectMoneyLaundering.Models;

namespace DetectMoneyLaundering.Interfaces;

public interface IMoneyLaunderingService
{
    Task<Account?> GetAccount(int id);
    Task<IEnumerable<Account>> GetAllAccounts();
    Task<InspectAccountModel> InspectAccount(
        int id,
        VisualizationModes mode,
        bool draw = true,
        string color = "",
        string backgroundColor = "",
        int slider = Parameters.ScalingDefault
    );
    Task<List<InspectAccountModel>> InspectAllAccounts();
}
