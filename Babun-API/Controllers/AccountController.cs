using DataAccessLibrary.DataServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Babun_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(DataAccountService dataAccountService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var test = await dataAccountService.GetAsync(id);

            Console.WriteLine(test);

            return Ok(test.AccountId);
        }
    }
}
