using Babun_API.Data;
using Babun_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Babun_API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AdController(ApiContext dbContext) : ControllerBase
{
    [HttpGet(Name = "GetAd")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await dbContext.Ads.FindAsync(id);
        if (result == null)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpGet]
    [ActionName("GetAll")]
    public async Task<IActionResult> Get()
    {
        var result = await dbContext.Ads.ToListAsync();
        if (result == null)
            return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Testing here hehehhe
    /// </summary>
    /// <remarks>
    /// Please do not do anything stupid
    ///
    /// Sample request:
    /// POST /api/Ad/Create
    /// {
    ///     "title" : "Sample Title",
    ///     "content" : "Content",
    ///     "author" : "Author Author"
    /// }
    /// </remarks>
    /// <param name="ad">the ad to create</param>
    /// <returns>a json response code indicating yes/no etc...</returns>
    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(Ad ad)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            await dbContext.AddAsync(ad);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return new JsonResult(Ok(ad));
    }

    // [HttpPost]
    // public async Task<IActionResult> Update(Ad ad, Ad updatedAd)
    // {
    //     try
    //     {
    //         dbContext.Set<Ad>().Entry(ad).CurrentValues.SetValues(updatedAd);
    //         await dbContext.SaveChangesAsync();
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //     }
    //
    //     return new JsonResult(Ok(ad));
    // }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var ad = await dbContext.Ads.FindAsync(id);
        dbContext.Remove(ad);
        await dbContext.SaveChangesAsync();
        return new JsonResult(Ok());
    }
}
