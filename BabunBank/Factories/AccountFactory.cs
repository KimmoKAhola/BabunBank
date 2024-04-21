using DataAccessLibrary.Data;

namespace BabunBank.Factories;

public static class AccountFactory
{
    public static Account CreateNewAccount()
    {
        var account = new Account
        {
            Frequency = "AfterTransaction",
            Created = DateOnly.FromDateTime(DateTime.Now),
            Balance = 0m
        };

        return account;
    }
}
