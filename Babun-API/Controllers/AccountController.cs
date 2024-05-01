using Asp.Versioning;
using AutoMapper;
using BabunBank.Models.ViewModels.Account;
using BabunBank.Models.ViewModels.Transaction;
using DataAccessLibrary.DataServices;
using Microsoft.AspNetCore.Mvc;

namespace Babun_API.Controllers;

/// <summary>
/// AccountController class handles API endpoints related to account operations.
/// </summary>
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api")]
[ApiController]
public class AccountController(DataAccountService dataAccountService, IMapper mapper)
    : ControllerBase
{
    /// <summary>
    /// Retrieves an account by its ID.
    /// </summary>
    /// <param name="id">The ID of the account to retrieve.</param>
    /// <returns>The account with the specified ID.</returns>
    [HttpGet("/me/{id:int}")]
    public async Task<IActionResult> Me(int id)
    {
        try
        {
            var account = await dataAccountService.GetAsync(id);
            if (account == null)
            {
                return NotFound($"Account with ID {id} not found");
            }

            var accountViewModel = mapper.Map<AccountViewModel>(account);

            return Ok(accountViewModel);
        }
        catch (Exception ex)
        {
            // Log the exception here
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                $"Error retrieving data from the database"
            );
        }
    }

    /// <summary>
    /// Retrieves account transactions by account ID, limit, offset, and sort order.
    /// </summary>
    /// <param name="id">The ID of the account to retrieve transactions for.</param>
    /// <param name="limit">The maximum number of transactions to retrieve.</param>
    /// <param name="offset">The offset used for pagination.</param>
    /// <param name="desc">The sorting order. Default value of true.</param>
    /// <returns>A list of transactions for the specified account ID, limit, and offset.</returns>
    [ProducesResponseType(typeof(TransactionViewModel), 200)]
    [ProducesResponseType(400)]
    [HttpGet("/account/{id:int}/{limit:int}/{offset:int}")]
    public async Task<IActionResult> Account(int id, int limit, int offset, bool? desc = true)
    {
        if (id <= 0 || limit <= 0 || offset < 0)
        {
            return BadRequest("Wrong parameters provided");
        }
        try
        {
            var account = await dataAccountService.GetAsync(id);
            if (account == null)
            {
                return BadRequest("Wrong id provided");
            }
            var result =
                desc.HasValue && desc.Value
                    ? account
                        .Transactions.OrderByDescending(x => x.TransactionId)
                        .Skip(offset)
                        .Take(limit)
                        .ToList()
                    : account
                        .Transactions.OrderBy(x => x.TransactionId)
                        .Skip(offset)
                        .Take(limit)
                        .ToList();
            var accountViewModel = mapper.Map<List<TransactionViewModel>>(result);

            return Ok(accountViewModel);
        }
        catch (Exception e)
        {
            return BadRequest("Wrong parameters provided");
        }
    }
}
