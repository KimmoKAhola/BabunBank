using BabunBank.Models.User;
using BabunBank.Services;
using Microsoft.AspNetCore.Mvc;

namespace BabunBank.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly UserService _userService;

    public UserController(ILogger<UserController> logger, UserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var userEntities = await _userService.GetAllUsersAsync();

        var viewModel = userEntities.Select(userEntity => new UserViewModel
        {
            FirstName = userEntity.FirstName,
            LastName = userEntity.LastName
        });

        var model = await _userService.GetAllUsersViewModelAsync();

        return View(model);
    }
}