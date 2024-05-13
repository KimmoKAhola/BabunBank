using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.Data;

public enum UserRole
{
    Choose,
    Admin,
    Cashier
}

public class DataInitializer(BankAppDataContext dbContext, UserManager<IdentityUser> userManager)
{
    public void SeedData()
    {
        dbContext.Database.Migrate();
        SeedRoles();
        SeedUsers();
    }

    private void SeedUsers()
    {
        AddUserIfNotExists("richard.chalk@systementor.se", "Hejsan123#", [UserRole.Admin]);
        AddUserIfNotExists("richard.erdos.chalk@gmail.se", "Hejsan123#", [UserRole.Cashier]);
        AddUserIfNotExists("bjorn@mail.se", "Hejsan123#", [UserRole.Admin]);
    }

    private void SeedRoles()
    {
        AddRoleIfNotExisting(UserRole.Admin.ToString());
        AddRoleIfNotExisting(UserRole.Cashier.ToString());
    }

    private void AddRoleIfNotExisting(string roleName)
    {
        var role = dbContext.Roles.FirstOrDefault(r => r.Name == roleName);
        if (role != null)
            return;
        dbContext.Roles.Add(new IdentityRole { Name = roleName, NormalizedName = roleName });
        dbContext.SaveChanges();
    }

    private void AddUserIfNotExists(string userName, string password, UserRole[] roles)
    {
        if (userManager.FindByEmailAsync(userName).Result != null)
            return;

        var user = new IdentityUser
        {
            UserName = userName,
            Email = userName,
            EmailConfirmed = true
        };

        userManager.CreateAsync(user, password).Wait();

        var roleNames = roles.Select(role => role.ToString()).ToArray();

        userManager.AddToRolesAsync(user, roleNames).Wait();
    }
}
