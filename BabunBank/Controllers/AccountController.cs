using BabunBank.Factories;
using BabunBank.Infrastructure.Enums;
using BabunBank.Infrastructure.Interfaces;
using BabunBank.Models.FormModels.TransferModels;
using BabunBank.Models.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

[Authorize(Roles = $"{UserRoleNames.Admin}, {UserRoleNames.Cashier}")]
public class AccountController(
    IAccountService accountService,
    ITransactionService transactionService,
    ITransactionFactory transactionFactory
) : Controller
{
    private const int StartingPage = 1;
    private const int DefaultPageSize = 50;

    /// <summary>
    /// Presents the details for a single customer and the customers account.
    /// </summary>
    /// <param name="id">The accound Id</param>
    /// <returns>A view with an account view model</returns>
    public async Task<IActionResult> Details(int id)
    {
        var accountViewModel = await accountService.GetAccountViewModelAsync(id);

        if (IsInvalidAccountViewModel(accountViewModel))
            return RedirectToAction("Index", "Error");

        return View(accountViewModel);
    }

    public async Task<IActionResult> Deposit(int id)
    {
        var accountViewModel = await accountService.GetAccountViewModelAsync(id);

        if (IsInvalidAccountViewModel(accountViewModel))
            return RedirectToAction("Index", "Error");

        var depositModel = CreateTransferModelFactory.CreateDepositModel(accountViewModel);

        return View(depositModel);
    }

    [HttpPost]
    [ActionName("Deposit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deposit(
        int id,
        [Bind("AccountId, Date, Type, Operation, Amount, Balance, Symbol, Bank")]
            CreateDepositModel depositModel
    )
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Incorrect values provided.";
            return View(depositModel);
        }

        var accountViewModel = await accountService.GetAccountViewModelAsync(id);

        if (IsInvalidAccountViewModel(accountViewModel))
            return RedirectToAction("Index", "Error");

        var transaction = transactionFactory.CreateDeposit(depositModel); //Har skapat en transaction
        if (await transactionService.CreateDepositAsync(transaction) == null)
            return RedirectToAction("Index", "Error"); //Something went wrong

        return RedirectToAction("Details", new { id = accountViewModel.AccountId });
    }

    public async Task<IActionResult> Withdraw(int id)
    {
        var account = await accountService.GetAccountViewModelAsync(id);
        if (account == null)
            return RedirectToAction("Index", "Error");

        var model = CreateTransferModelFactory.CreateWithdrawalModel(account);

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

        var withdrawAccountViewModel = await accountService.GetAccountViewModelAsync(id);

        if (withdrawAccountViewModel == null)
            return RedirectToAction("Index", "Error");

        var withdrawal = transactionFactory.CreateWithdrawal(withdrawalModel);

        if (await transactionService.CreateWithdrawalAsync(withdrawal) == null)
            return RedirectToAction("Index", "Error");

        return RedirectToAction("Details", new { id = withdrawAccountViewModel.AccountId });
    }

    public async Task<IActionResult> Transfer(
        int id,
        int pageNumber = StartingPage,
        int pageSize = DefaultPageSize,
        string q = ""
    )
    {
        // totalPageCount = (int)Math.Ceiling((double)totalPageCount / pageSize);

        ViewBag.CurrentPage = pageNumber;
        ViewBag.Q = q;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalPageCount = 0;

        var senderAccountViewModel = await accountService.GetAccountViewModelAsync(id);

        if (senderAccountViewModel == null)
            return RedirectToAction("Index", "Error");

        var transferModel = CreateTransferModelFactory.CreateTransferModel(senderAccountViewModel);

        // ViewBag.ListOfCustomers = await accountService.RenameMe(id, pageNumber, pageSize, q);
        await GetListOfCustomers(transferModel, q, pageNumber, pageSize);
        return View(transferModel);
    }

    [HttpPost]
    [ActionName("Transfer")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Transfer(
        CreateTransferModel transferModel,
        string q = "",
        int pageNumber = StartingPage,
        int pageSize = DefaultPageSize
    )
    {
        await GetListOfCustomers(transferModel, q, pageNumber, pageSize);

        ViewBag.CurrentPage = pageNumber;
        ViewBag.Q = q;
        ViewBag.PageSize = pageSize;

        if (!ModelState.IsValid)
            return View(transferModel);

        var receivingAccountViewModel = await accountService.GetAccountViewModelAsync(
            transferModel.ToAccountId
        );

        if (IsInvalidAccountViewModel(receivingAccountViewModel))
            return View(transferModel);

        var moneyTransfer = transactionFactory.CreateTransfer(
            transferModel,
            receivingAccountViewModel
        );

        var resultOfTransfer = await transactionService.CreateTransferAsync(moneyTransfer);

        if (resultOfTransfer is null or false)
            return View(transferModel); //TODO return error smth went wrong

        return RedirectToAction("Details", new { id = transferModel.FromAccountId });
    }

    private async Task GetListOfCustomers(
        CreateTransferModel transferModel,
        string q,
        int pageNumber,
        int pageSize
    )
    {
        ViewBag.ListOfCustomers = await accountService.RenameMe(
            transferModel.FromAccountId,
            pageNumber,
            pageSize,
            q
        );
    }

    public async Task<IActionResult> Filter(
        CreateTransferModel transferModel,
        string q = "",
        int pageNumber = 1,
        int pageSize = 50
    )
    {
        // ViewBag.ListOfCustomers = await accountService.RenameMe(
        //     model.FromAccountId,
        //     pageNumber,
        //     pageSize,
        //     q
        // );

        await GetListOfCustomers(transferModel, q, pageNumber, pageSize);

        ViewBag.CurrentPage = pageNumber;
        ViewBag.Q = q;
        ViewBag.PageSize = pageSize;

        return RedirectToAction(
            "Transfer",
            new
            {
                id = transferModel.FromAccountId,
                pageNumber = ViewBag.CurrentPage,
                pageSize = ViewBag.PageSize,
                q = ViewBag.Q
            }
        );
    }

    public IActionResult Clear(CreateTransferModel transferModel)
    {
        ViewBag.CurrentPage = 0;
        ViewBag.Q = "";
        ViewBag.PageSize = 50;

        return RedirectToAction(
            "Transfer",
            new
            {
                id = transferModel.FromAccountId,
                pageNumber = ViewBag.CurrentPage,
                pageSize = ViewBag.PageSize,
                q = ViewBag.Q
            }
        );
    }

    private static bool IsInvalidAccountViewModel(AccountViewModel? accountViewModel)
    {
        return accountViewModel == null;
    }
}
