using Asp.Versioning;
using AutoMapper;
using BabunBank.Models.FormModels.Api;
using Babun_API.Data;
using Babun_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Babun_API.Controllers;

/// <summary>
/// A controller for handling ads. The ads can be used at blogposts however you wish.
/// </summary>
/// <param name="dbContext">The relevant database context</param>
/// <param name="mapper">Automapper to convert between data transfer objects.</param>
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "v1")]
[Route("[controller]")]
[ProducesResponseType(401)]
[ProducesResponseType(403)]
[ProducesResponseType(500)]
[ProducesResponseType(503)]
[ApiController]
public class AdsController(ApiContext dbContext, IMapper mapper) : ControllerBase
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
    [ApiVersion("1.0")]
    [ProducesResponseType(typeof(ViewAdModel), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get(int id)
    {
        var result = await dbContext.Ads.FindAsync(id);

        if (result == null)
            return NotFound("The ad was not found.");

        var adViewModel = mapper.Map<Ads, ViewAdModel>(result);

        return Ok(adViewModel);
    }

    /// <summary>
    /// Obtain all database objects.
    /// </summary>
    /// <returns>Returns all available database objects, deleted and non-deleted (soft delete implemented).</returns>
    [HttpGet]
    [ApiVersion("1.0")]
    [ActionName("GetAll")]
    [ProducesResponseType(typeof(List<ViewAdModel>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Get()
    {
        var result = await dbContext.Ads.ToListAsync();

        if (result == null)
            return BadRequest("No ads were found.");

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
    [ApiVersion("1.0")]
    [ProducesResponseType(typeof(ViewAdModel), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
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

            var adViewModel = mapper.Map<ViewAdModel>(ad);

            return Ok(adViewModel);
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
    [Authorize(AuthenticationSchemes = "V1Scheme")]
    [ApiVersion("1.0")]
    [ProducesResponseType(typeof(ViewAdModel), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] EditAdModel model)
    {
        if (id != model.id)
        {
            return BadRequest("Wrong id provided.");
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var itemToUpdate = await dbContext.Ads.FindAsync(id);
        if (itemToUpdate == null)
            return NotFound("Ad not found.");

        try
        {
            itemToUpdate.Title = model.title;
            itemToUpdate.Author = model.author;
            itemToUpdate.Description = model.description;
            itemToUpdate.Content = model.content;
            itemToUpdate.IsDeleted = model.isDeleted;
            itemToUpdate.LastModified = DateTime.Now;

            await dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var updatedAd = await dbContext.Ads.FindAsync(id);

        var adViewModel = mapper.Map<ViewAdModel>(updatedAd);

        return Ok(adViewModel);
    }

    /// <summary>
    /// Soft deletes a specific Ad.
    /// </summary>
    /// <param name="id">The id of the database object to delete.</param>
    /// <returns>BadRequest if the database minimum amount of 100 has been reached.
    /// NotFound if the ad does not exist.
    /// Status code 500 if the database fails.</returns>
    [Authorize(AuthenticationSchemes = "V1Scheme")]
    [ApiVersion("1.0")]
    [HttpDelete("{id}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
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
            return Ok("Deletion was successful.");
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
