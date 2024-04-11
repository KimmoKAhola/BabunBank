// See https://aka.ms/new-console-template for more information

using System.Globalization;
using DataAccessLibrary.Data;
using DetectMoneyLaundering.Services;
using Microsoft.EntityFrameworkCore;

using (var dbContext = new BankAppDataContext())
{
    var test = new MoneyLaunderingService(dbContext);

    var result = await test.GetAccount(2);

    // var sus = test.InspectAccount(result);
    //
    // foreach (var transaction in sus.TransactionsOverLimit)
    // {
    //     Console.WriteLine("Transaction Id: " + transaction.TransactionId);
    //     Console.WriteLine(
    //         "Transaction Amount: " + transaction.Amount.ToString("C2", new CultureInfo("sv-SE"))
    //     );
    // }

    Console.ReadKey();
}
