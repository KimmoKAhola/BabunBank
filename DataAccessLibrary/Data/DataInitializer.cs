using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.Data;

public enum UserRoles
{
    Admin,
    Customer, //TODO remove this later
    Cashier
}

public class DataInitializer
{
    private readonly BankAppDataContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public DataInitializer(BankAppDataContext dbContext, UserManager<IdentityUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }
    public async void SeedData()
    {
        await _dbContext.Database.MigrateAsync();
        SeedRoles();
        SeedUsers();
    }

    // Här finns möjlighet att uppdatera dina användares loginuppgifter
    private void SeedUsers()
    {
        AddUserIfNotExists("richard.chalk@systementor.se", "Hejsan123#", new UserRoles[] { UserRoles.Admin });
        AddUserIfNotExists("richard.chalk@customer.systementor.se", "Hejsan123#", new UserRoles[] { UserRoles.Customer}); //TODO this should be removed later
        AddUserIfNotExists("richard.erdos.chalk@gmail.se", "Hejsan123#", new UserRoles[]{UserRoles.Cashier});
        AddUserIfNotExists("bjorn@mail.se", "Hejsan123#", new UserRoles[] { UserRoles.Admin});
    }

    // Här finns möjlighet att uppdatera dina användares roller
    private void SeedRoles()
    {
        AddRoleIfNotExisting(UserRoles.Admin.ToString());
        AddRoleIfNotExisting(UserRoles.Cashier.ToString());
        AddRoleIfNotExisting(UserRoles.Customer.ToString());  // TODO remove this role later
    }

    private async void AddRoleIfNotExisting(string roleName)
    {
        var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        if (role != null) return;
        _dbContext.Roles.Add(new IdentityRole { Name = roleName, NormalizedName = roleName });
        await _dbContext.SaveChangesAsync();
    }

    private void AddUserIfNotExists(string userName, string password, UserRoles[] roles)
    {
        if (_userManager.FindByEmailAsync(userName).Result != null) return;

        var user = new IdentityUser
        {
            UserName = userName,
            Email = userName,
            EmailConfirmed = true
        };
        _userManager.CreateAsync(user, password).Wait();

        string[] roleNames = roles.Select(role => Enum.GetName(typeof(UserRoles), role)).ToArray();
        
        _userManager.AddToRolesAsync(user, roleNames).Wait();
    }
}