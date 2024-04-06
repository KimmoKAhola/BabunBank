using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.Data;

public enum UserRole
{
    Admin,
    Customer, //TODO remove this later
    Cashier
}

public class DataInitializer(BankAppDataContext dbContext, UserManager<IdentityUser> userManager)
{
    public async Task SeedData()
    {
        await dbContext.Database.MigrateAsync();
        await SeedRoles();
        await SeedUsers();
    }

    // Här finns möjlighet att uppdatera dina användares loginuppgifter
    private async Task SeedUsers()
    {
        await AddUserIfNotExists(
            "richard.chalk@systementor.se",
            "Hejsan123#",
            new UserRole[] { UserRole.Admin }
        );
        await AddUserIfNotExists(
            "richard.chalk@customer.systementor.se",
            "Hejsan123#",
            new UserRole[] { UserRole.Customer }
        ); //TODO this should be removed later
        await AddUserIfNotExists(
            "richard.erdos.chalk@gmail.se",
            "Hejsan123#",
            new UserRole[] { UserRole.Cashier }
        );
        await AddUserIfNotExists("bjorn@mail.se", "Hejsan123#", new UserRole[] { UserRole.Admin });
    }

    // Här finns möjlighet att uppdatera dina användares roller
    private async Task SeedRoles()
    {
        await AddRoleIfNotExisting(UserRole.Admin.ToString());
        await AddRoleIfNotExisting(UserRole.Cashier.ToString());
        await AddRoleIfNotExisting(UserRole.Customer.ToString()); // TODO remove this role later
    }

    private async Task AddRoleIfNotExisting(string roleName)
    {
        var role = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        if (role != null)
            return;
        dbContext.Roles.Add(new IdentityRole { Name = roleName, NormalizedName = roleName });
        await dbContext.SaveChangesAsync();
    }

    private async Task AddUserIfNotExists(string userName, string password, UserRole[] roles)
    {
        if (userManager.FindByEmailAsync(userName).Result != null)
            return;

        var user = new IdentityUser
        {
            UserName = userName,
            Email = userName,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(user, password);

        string[] roleNames = roles.Select(role => role.ToString()).ToArray();

        await userManager.AddToRolesAsync(user, roleNames);
    }
}
