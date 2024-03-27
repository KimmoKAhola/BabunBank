using BabunBank.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BabunBank.Data;
using BabunBank.Models.LandingPage;
using Microsoft.EntityFrameworkCore;

namespace BabunBank.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public LandingPageViewModel _landingPageViewModel;
        private readonly ApplicationDbContext _dbContext;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            // _landingPageViewModel = landingPageViewModel;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var query = _dbContext.Accounts
                .Include(x => x.Dispositions)
                .ThenInclude(x => x.Customer);

            _landingPageViewModel = new LandingPageViewModel
            {
                NumberOfAccounts = query.Count(),
                NumberOfCustomers = query.SelectMany(x => x.Dispositions.Select(d => d.CustomerId)).Distinct().Count(),
                TotalAccountBalance = query.Sum(x => x.Balance)
            };
            
            return View(_landingPageViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
