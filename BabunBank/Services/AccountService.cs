﻿using AutoMapper;
using BabunBank.Infrastructure.Interfaces;
using BabunBank.Models.ViewModels.Account;
using DataAccessLibrary.Data;
using DataAccessLibrary.DataServices;

namespace BabunBank.Services;

public class AccountService(DataAccountService dataAccountService, IMapper mapper) : IAccountService
{
    private const int ItemsPerPage = 50;
    private const int ItemsPerTransferPage = 10;

    public async Task<AccountViewModel?> GetAccountViewModelAsync(int id)
    {
        var result = await dataAccountService.GetAsync(id);

        if (result == null)
            return null!;

        var accountViewModel = mapper.Map<AccountViewModel>(result);
        accountViewModel.CustomerId = result
            .Dispositions.First(x => x.Type.ToUpper() == "OWNER")
            .CustomerId;

        accountViewModel.Transactions = accountViewModel
            ?.Transactions?.OrderByDescending(t => t.TransactionId)
            .Take(ItemsPerPage)
            .ToList();
        return accountViewModel;
    }

    public async Task<AccountViewModel?> GetTransferAccountViewModelAsync(int id)
    {
        var result = await dataAccountService.GetAsync(id);

        if (result == null)
            return null!;

        var accountViewModel = mapper.Map<AccountViewModel>(result);
        accountViewModel.CustomerId = result
            .Dispositions.First(x => x.Type.ToUpper() == "OWNER")
            .CustomerId;

        accountViewModel.Transactions = accountViewModel
            ?.Transactions?.OrderByDescending(t => t.TransactionId)
            .Take(ItemsPerTransferPage)
            .ToList();
        return accountViewModel;
    }

    public async Task<(
        IEnumerable<Account> listOfAccounts,
        int NumberOfAccounts
    )> GetAccountsAndNumberOfAccounts(int id, int pageNumber, int pageSize, string q)
    {
        var result = await dataAccountService.GetAllAccountsAndCustomersAsync(
            id,
            pageNumber,
            pageSize,
            q
        );

        return (result.listOfAccounts, result.numberOfAccounts);
    }
}
