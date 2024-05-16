namespace BabunBank.Factories.Account;

public static class AccountFactory
{
    private const decimal InitialBalance = 0m;

    public static DataAccessLibrary.Data.Account Create()
    {
        return new DataAccessLibrary.Data.Account
        {
            Frequency = "AfterTransaction", //TODO add as parameter
            Created = DateOnly.FromDateTime(DateTime.Now),
            Balance = InitialBalance
        };
    }
}
