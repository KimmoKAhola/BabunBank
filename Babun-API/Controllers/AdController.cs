using Asp.Versioning;
using AutoMapper;
using BabunBank.Models.FormModels.AdModels;
using Babun_API.Data;
using Babun_API.Infrastructure.Configurations;
using Babun_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Babun_API.Controllers;

/// <summary>
/// Version 2 of the API. This requires the Web abb to be used.
/// </summary>
/// <param name="dbContext"></param>
[ApiVersion("2.0")]
[ApiExplorerSettings(GroupName = SwaggerConfiguration.Version2)]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize(
    AuthenticationSchemes = SwaggerConfiguration.V2Scheme,
    Policy = SwaggerConfiguration.V2Policy
)]
[ProducesResponseType(401)]
[ProducesResponseType(403)]
[ProducesResponseType(500)]
[ProducesResponseType(503)]
public class AdController(ApiContext dbContext, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Fetches a singular ad by its database ID.
    /// </summary>
    /// <param name="id">The ID of the ad to fetch.</param>
    /// <returns>
    /// Returns an HTTP status code 200 (OK) along with the ad object if found,
    /// or an HTTP status code 404 (Not Found) if the ad with the specified ID does not exist.
    /// </returns>
    [HttpGet("<id>")]
    [ProducesResponseType(typeof(ViewAdModel), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get(int id)
    {
        var result = await dbContext.Ads.FindAsync(id);

        if (result == null)
            return NotFound("No ad was found.");

        var adViewModel = mapper.Map<Ads, ViewAdModel>(result);

        return Ok(adViewModel);
    }

    /// <summary>
    /// Obtain all database objects.
    /// </summary>
    /// <returns>Returns all available database objects, deleted and non-deleted (soft delete implemented).</returns>
    [HttpGet]
    [ActionName("GetAll")]
    [ProducesResponseType(typeof(List<ViewAdModel>), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get()
    {
        var result = await dbContext.Ads.ToListAsync();

        if (result == null)
            return NotFound("No ads were found.");

        var ads = mapper.Map<IEnumerable<Ads>, IEnumerable<ViewAdModel>>(result);

        return Ok(ads);
    }

    /// <summary>
    /// Create a new database object.
    /// </summary>
    /// <remarks>
    /// Please do not do anything stupid. The database has a limit of 500 rows
    /// </remarks>
    /// <param name="model">A create ad model form</param>
    /// <returns>Ok(object), return BadRequest if limit is reached, returns status code 500 if the save to database fails.</returns>
    ///
    [HttpPost]
    [ProducesResponseType(typeof(ViewAdModel), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Create(CreateAdModel model)
    {
        if (await dbContext.Ads.CountAsync() > 500)
        {
            return BadRequest("Database limit of 500 has been reached. Try to delete something!");
        }
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var ad = mapper.Map<Ads>(model);

            await dbContext.AddAsync(ad);
            await dbContext.SaveChangesAsync();

            var viewAd = mapper.Map<ViewAdModel>(ad);
            return Ok(viewAd);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Update a database object to something else.
    /// Authorization required with a JWT token.
    /// </summary>
    /// <param name="id">The id of the database object to update</param>
    /// <param name="model">Your model of the object to update. See schema requirements below</param>
    /// <returns>BadRequest if id is incorrect. Invalid BadRequest if model state is incorrect.
    /// NotFound if the item does not exist. Status code 500 if the database fails.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Update(int id, [FromBody] EditAdModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var itemToUpdate = await dbContext.Ads.FindAsync(id);
        if (itemToUpdate == null)
            return NotFound("Ad not found.");

        try
        {
            itemToUpdate = mapper.Map(model, itemToUpdate);
            itemToUpdate.LastModified = DateTime.Now;

            dbContext.Update(itemToUpdate);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var updatedAd = await dbContext.Ads.FindAsync(id);

        return Ok(updatedAd);
    }

    /// <summary>
    /// Updates a singular ad specified by its ID with the provided patch document.
    /// </summary>
    /// <param name="id">The ID of the ad to update.</param>
    /// <param name="patchDocument">The JSON patch document containing the updates to apply.</param>
    /// <returns>
    /// Returns an HTTP status code 200 (OK) if the ad is successfully updated,
    /// an HTTP status code 400 (Bad Request) if the patch document is null or the ID is invalid,
    /// an HTTP status code 404 (Not Found) if the ad with the specified ID does not exist,
    /// an HTTP status code 409 (Conflict) if there is a concurrency conflict during the update,
    /// an HTTP status code 500 (Internal Server Error) if an error occurs during the update.
    /// </returns>
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(
        int id,
        [FromBody] JsonPatchDocument<EditAdModel>? patchDocument
    )
    {
        if (patchDocument == null || id <= 0)
        {
            return BadRequest();
        }

        var existingAd = await dbContext.Ads.FindAsync(id);

        if (existingAd == null)
        {
            return BadRequest();
        }

        var editAdModel = mapper.Map<EditAdModel>(existingAd);
        patchDocument.ApplyTo(editAdModel, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        existingAd = mapper.Map(editAdModel, existingAd);
        existingAd.LastModified = DateTime.Now;

        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            dbContext.Update(existingAd);
            await dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            await transaction.RollbackAsync();
            return Conflict();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await transaction.RollbackAsync();
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok();
    }

    /// <summary>
    /// Soft deletes a specific Ad.
    /// </summary>
    /// <param name="id">The id of the database object to delete.</param>
    /// <returns>BadRequest if the database minimum amount of 100 has been reached.
    /// NotFound if the ad does not exist.
    /// Status code 500 if the database fails.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Delete(int id)
    {
        if (await dbContext.Ads.CountAsync() < 100)
        {
            return BadRequest(
                "Database minimum limit of 100 has been reached. Try to create something!"
            );
        }
        var ad = await dbContext.Ads.FindAsync(id);
        if (ad == null)
            return NotFound("The ad was not found.");

        try
        {
            ad.IsDeleted = true;
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
