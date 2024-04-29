using Asp.Versioning;
using AutoMapper;
using BabunBank.Models.FormModels.Ad;
using Babun_API.Data;
using Babun_API.Infrastructure.Configurations;
using Babun_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Babun_API.Controllers;

/// <summary>
/// A controller for handling ads. The ads can be used at blogposts however you wish.
/// </summary>
/// <param name="dbContext">The relevant database context</param>
/// <param name="mapper">Automapper to convert between data transfer objects.</param>
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = SwaggerConfiguration.Version1)]
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

    // /// <summary>
    // /// This method returns a list of views for Ad Models.
    // /// HTTP status code 200 is returned if successful, otherwise 400.
    // /// </summary>
    // /// <param name="isDeleted">Optional. If specified and true, the result will contain only deleted ads.</param>
    // /// <returns>A Task that represents the asynchronous operation. The task result contains the IActionResult.</returns>
    // [HttpGet]
    // [ApiVersion("1.0")]
    // [ActionName("GetAll")]
    // [ProducesResponseType(typeof(List<ViewAdModel>), 200)]
    // [ProducesResponseType(400)]
    // public async Task<IActionResult> Get(bool? isDeleted)
    // {
    //     var result = await dbContext.Ads.ToListAsync();
    //
    //     if (isDeleted.GetValueOrDefault())
    //     {
    //         result = result.Where(x => x.IsDeleted).ToList();
    //         var resultAds = mapper.Map<IEnumerable<Ads>, IEnumerable<ViewAdModel>>(result);
    //         return Ok(resultAds);
    //     }
    //
    //     result = result.Where(x => !x.IsDeleted).ToList();
    //
    //     if (result == null)
    //         return BadRequest("No ads were found.");
    //
    //     var ads = mapper.Map<IEnumerable<Ads>, IEnumerable<ViewAdModel>>(result);
    //
    //     return Ok(ads);
    // }


    [HttpGet]
    [ApiVersion("1.0")]
    [ActionName("GetAll")]
    [ProducesResponseType(typeof(List<ViewAdModel>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get(bool isDeleted)
    {
        var ads = await dbContext.Ads.ToListAsync();

        var resultAds = mapper.Map<IEnumerable<Ads>, IEnumerable<ViewAdModel>>(ads);

        resultAds = isDeleted
            ? resultAds.Where(x => x.IsDeleted).ToList()
            : resultAds.Where(x => !x.IsDeleted).ToList();

        return Ok(resultAds);
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
    [Authorize(AuthenticationSchemes = SwaggerConfiguration.V1Scheme)]
    [ApiVersion("1.0")]
    [ProducesResponseType(typeof(ViewAdModel), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    [HttpPut("{id:int}")]
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
        catch (Exception e)
        {
            Console.WriteLine(e + e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var updatedAd = await dbContext.Ads.FindAsync(id);

        var adViewModel = mapper.Map<ViewAdModel>(updatedAd);

        return Ok(adViewModel);
    }

    /// <summary>
    /// Updates a partial ad by its ID with the specified patch document.
    /// </summary>
    /// <param name="id">The ID of the ad to update.</param>
    /// <param name="patchDocument">The patch document containing the partial updates.</param>
    /// <returns>
    /// Returns an HTTP status code 200 (OK) if the ad is successfully updated,
    /// an HTTP status code 400 (Bad Request) if the patch document or ID is invalid,
    /// or an HTTP status code 404 (Not Found) if the ad with the specified ID does not exist.
    /// </returns>
    [Authorize(AuthenticationSchemes = SwaggerConfiguration.V1Scheme)]
    [ApiVersion("1.0")]
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateAdPartial(
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

        dbContext.Update(existingAd);
        await dbContext.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Soft deletes a specific Ad.
    /// </summary>
    /// <param name="id">The id of the database object to delete.</param>
    /// <returns>BadRequest if the database minimum amount of 100 has been reached.
    /// NotFound if the ad does not exist.
    /// Status code 500 if the database fails.</returns>
    [Authorize(AuthenticationSchemes = SwaggerConfiguration.V1Scheme)]
    [ApiVersion("1.0")]
    [HttpDelete("{id:int}")]
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
