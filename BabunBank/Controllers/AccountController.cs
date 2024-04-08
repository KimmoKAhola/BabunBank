using BabunBank.Configurations.Enums;
using BabunBank.Factories;
using BabunBank.Models.Account;
using BabunBank.Models.Cashier;
using BabunBank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

[Authorize(Roles = $"{UserRoleNames.Admin}, {UserRoleNames.Cashier}")]
public class AccountController(AccountService accountService, TransactionService transactionService)
    : Controller
{
    public async Task<IActionResult> Details(int id)
    {
        var result = await accountService.GetAccountViewModelAsync(id);
        if (result == null)
            return RedirectToAction("Index", "Error");
        return View(result);
    }

    public async Task<IActionResult> Deposit(int id)
    {
        var account = await accountService.GetAccountViewModelAsync(id);
        if (account == null)
            return RedirectToAction("Index", "Error");

        var model = new CreateDepositModel
        {
            AccountId = account.AccountId,
            Balance = account.Balance,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Type = "Debit",
            Operation = "Deposit"
        };

        return View(model);
    }

    [HttpPost, ActionName("Deposit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deposit(
        int id,
        [Bind("AccountId, Date, Type, Operation, Amount,Balance,Symbol, Bank")]
            CreateDepositModel depositModel
    )
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Incorrect values provided.";
            return View(depositModel);
        }

        var account = await accountService.GetAccountViewModelAsync(id);
        if (account == null)
        {
            return RedirectToAction("Index", "Error");
        }
        var transaction = TransactionFactory.CreateDeposit(depositModel); //Har skapat en transaction
        if (await transactionService.CreateDepositAsync(transaction) == null)
        {
            return RedirectToAction("Index", "Error"); //Something went wrong
        }

        return RedirectToAction("Details", new { id = account.AccountId });
    }

    public async Task<IActionResult> Withdraw()
    {
        return View();
    }

    public async Task<IActionResult> Transfer()
    {
        return View();
    }
}
