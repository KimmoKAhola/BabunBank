using BabunBank.Configurations.Enums;
using BabunBank.Factories;
using BabunBank.Models.FormModels.Cashier;
using BabunBank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

[Authorize(Roles = $"{UserRoleNames.Admin}, {UserRoleNames.Cashier}")]
public class AccountController(AccountService accountService, TransactionService transactionService)
    : Controller
{
    /// <summary>
    /// Presents the details for a single customer and the customers account.
    /// </summary>
    /// <param name="id">The accound Id</param>
    /// <returns>A view with an account view model</returns>
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

    [HttpPost]
    [ActionName("Deposit")]
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
            return RedirectToAction("Index", "Error");
        var transaction = TransactionFactory.CreateDeposit(depositModel); //Har skapat en transaction
        if (await transactionService.CreateDepositAsync(transaction) == null)
            return RedirectToAction("Index", "Error"); //Something went wrong

        return RedirectToAction("Details", new { id = account.AccountId });
    }

    public async Task<IActionResult> Withdraw(int id)
    {
        var account = await accountService.GetAccountViewModelAsync(id);
        if (account == null)
            return RedirectToAction("Index", "Error");

        var model = new CreateWithdrawalModel
        {
            AccountId = account.AccountId,
            Balance = account.Balance,
            Date = DateOnly.FromDateTime(DateTime.Now),
            Type = "Debit",
            Operation = "Withdrawal"
        };

        return View(model);
    }

    [HttpPost]
    [ActionName("Withdraw")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Withdraw(int id, CreateWithdrawalModel withdrawalModel)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Incorrect values provided.";
            return View(withdrawalModel);
        }

        var account = await accountService.GetAccountViewModelAsync(id);
        if (account == null)
            return RedirectToAction("Index", "Error");

        var withdrawal = TransactionFactory.CreateWithdrawal(withdrawalModel);

        if (await transactionService.CreateWithdrawalAsync(withdrawal) == null)
            return RedirectToAction("Index", "Error");

        return RedirectToAction("Details", new { id = account.AccountId });
    }

    public async Task<IActionResult> Transfer(
        int id,
        int pageNumber = 0,
        int pageSize = 50,
        string q = ""
    )
    {
        if (ViewBag.Q != null)
        {
            q = ViewBag.Q;
        }

        if (ViewBag.CurrentPage != null)
        {
            pageNumber = ViewBag.CurrentPage;
        }

        if (ViewBag.PageSize != null)
        {
            pageSize = ViewBag.PageSize;
        }

        if (pageNumber == 0)
        {
            pageNumber = 1;
        }

        if (pageSize == 0)
        {
            pageSize = 50;
        }

        // totalPageCount = (int)Math.Ceiling((double)totalPageCount / pageSize);

        ViewBag.CurrentPage = pageNumber;
        ViewBag.Q = q;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalPageCount = 0;

        var account = await accountService.GetAccountViewModelAsync(id);

        if (account == null)
            return RedirectToAction("Index", "Error");

        var model = new CreateTransferModel
        {
            FromAccountId = account.CustomerId,
            BalanceSender = account.Balance
        };

        ViewBag.ListOfCustomers = await accountService.RenameMe(id, pageNumber, pageSize, q);
        return View(model);
    }

    [HttpPost]
    [ActionName("Transfer")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Transfer(CreateTransferModel transferModel)
    {
        string q = "";
        int pageNumber = 0;
        int pageSize = 50;

        ViewBag.ListOfCustomers = await accountService.RenameMe(
            transferModel.FromAccountId,
            pageNumber,
            pageSize,
            q
        );
        if (!ModelState.IsValid)
            return View(transferModel); //Error

        ViewBag.CurrentPage = pageNumber;
        ViewBag.Q = q;
        ViewBag.PageSize = pageSize;

        var receiver = await accountService.GetAccountViewModelAsync(transferModel.ToAccountId);

        var transfer = TransactionFactory.CreateTransfer(transferModel, receiver);

        var result = await transactionService.CreateTransferAsync(transfer);

        if (result is null or false)
            return View(); //Error

        return RedirectToAction("Details", new { id = transferModel.FromAccountId });
    }

    public async Task<IActionResult> Filter(
        CreateTransferModel model,
        int pageNumber,
        int pageSize,
        string q
    )
    {
        if (pageNumber == 0)
            pageNumber = 1;
        if (pageSize == 0)
            pageSize = 50;

        ViewBag.ListOfCustomers = await accountService.RenameMe(
            model.FromAccountId,
            pageNumber,
            pageSize,
            q
        );

        ViewBag.CurrentPage = pageNumber;
        ViewBag.Q = q;
        ViewBag.PageSize = pageSize;

        return RedirectToAction(
            "Transfer",
            new
            {
                id = model.FromAccountId,
                pageNumber = ViewBag.CurrentPage,
                pageSize = ViewBag.PageSize,
                q = ViewBag.Q
            }
        );
    }

    public IActionResult Clear(CreateTransferModel model)
    {
        ViewBag.CurrentPage = 0;
        ViewBag.Q = "";
        ViewBag.PageSize = 50;

        return RedirectToAction(
            "Transfer",
            new
            {
                id = model.FromAccountId,
                pageNumber = ViewBag.CurrentPage,
                pageSize = ViewBag.PageSize,
                q = ViewBag.Q
            }
        );
    }
}
