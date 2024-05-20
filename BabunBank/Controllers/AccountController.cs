using BabunBank.Factories.Transactions;
using BabunBank.Infrastructure.Enums;
using BabunBank.Infrastructure.Interfaces;
using BabunBank.Infrastructure.Parameters;
using BabunBank.Models.FormModels.Transactions;
using BabunBank.Models.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BabunBank.Controllers;

[Authorize(Roles = $"{UserRoleNames.Admin}, {UserRoleNames.Cashier}")]
public class AccountController(
    IAccountService accountService,
    ITransactionService transactionService,
    ITransactionFactory transactionFactory
) : Controller
{
    private const int StartingPage = 1;
    private const int DefaultPageSize = 20;

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
            return View(depositModel);
        }

        var accountViewModel = await accountService.GetAccountViewModelAsync(id);

        if (IsInvalidAccountViewModel(accountViewModel))
            return RedirectToAction("Index", "Error");

        var transaction = transactionFactory.CreateDeposit(depositModel);
        if (await transactionService.CreateDepositAsync(transaction) == null)
            return RedirectToAction("Index", "Error");

        TempData["Message"] = "Deposit Successful!";
        return RedirectToAction("Details", "Cashier", new { id = accountViewModel!.CustomerId });
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

        TempData["Message"] = "Withdrawal Successful!";
        return RedirectToAction("Details", "Cashier", new { id = withdrawalModel.CustomerId });
    }

    public async Task<IActionResult> Transfer(
        int id,
        int pageNumber = StartingPage,
        int pageSize = DefaultPageSize,
        string q = ""
    )
    {
        ViewBag.CurrentPage = pageNumber;
        ViewBag.Q = q;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalPageCount = 0;

        var senderAccountViewModel = await accountService.GetTransferAccountViewModelAsync(id);

        if (senderAccountViewModel == null)
            return RedirectToAction("Index", "Error");

        var transferModel = CreateTransferModelFactory.CreateTransferModel(senderAccountViewModel);

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
        {
            if (ModelState["ToAccountId"].Errors.Count > 0 && ModelState.ErrorCount == 1)
            {
                TempData["ErrorMessage"] = "You must select a customer.";
                var notification = TempData["ErrorMessage"] as string;
                if (!string.IsNullOrEmpty(notification))
                {
                    ViewBag.Notification = notification;
                }
            }
            return View(transferModel);
        }

        var receivingAccountViewModel = await accountService.GetTransferAccountViewModelAsync(
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
            return View(transferModel);

        TempData["Message"] = "Transfer Successful!";
        return RedirectToAction("Details", "Cashier", new { id = transferModel.FromCustomerId });
    }

    private async Task GetListOfCustomers(
        CreateTransferModel transferModel,
        string q,
        int pageNumber,
        int pageSize
    )
    {
        var result = await accountService.GetAccountsAndNumberOfAccounts(
            transferModel.FromAccountId,
            pageNumber,
            pageSize,
            q
        );
        var pageSizeValues = Enum.GetValues(typeof(CustomerDropdownMenu))
            .Cast<CustomerDropdownMenu>()
            .Select(x => new SelectListItem
            {
                Text = $"{((int)x).ToString()} results per page",
                Value = ((int)x).ToString()
            });

        ViewBag.ListOfCustomers = result.listOfAccounts;
        ViewBag.TotalPageCount = (int)Math.Ceiling((double)result.NumberOfAccounts / pageSize);
        ViewBag.PageSizeValues = pageSizeValues;
    }

    public async Task<IActionResult> Filter(
        CreateTransferModel transferModel,
        string q = "",
        int pageNumber = 1,
        int pageSize = 50
    )
    {
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

    [HttpPost]
    public IActionResult ClearFilters(CreateTransferModel transferModel)
    {
        ViewBag.CurrentPage = StartingPage;
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
