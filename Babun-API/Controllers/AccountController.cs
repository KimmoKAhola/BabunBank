using DataAccessLibrary.DataServices;
using Microsoft.AspNetCore.Mvc;

namespace Babun_API.Controllers;

/// <summary>
/// AccountController class handles API endpoints related to account operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AccountController(DataAccountService dataAccountService) : ControllerBase
{
    /// <summary>
    /// Retrieves an account by its ID.
    /// </summary>
    /// <param name="id">The ID of the account to retrieve.</param>
    /// <returns>The account with the specified ID.</returns>
    [HttpGet]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var account = await dataAccountService.GetAsync(id);
            if (account == null)
            {
                return NotFound($"Account with ID {id} not found");
            }

            return Ok(account.AccountId);
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
