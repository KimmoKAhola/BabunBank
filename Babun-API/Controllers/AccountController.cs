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
}
