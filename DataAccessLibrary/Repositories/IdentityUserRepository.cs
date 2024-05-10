using System.Linq.Expressions;
using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.Repositories;

public class IdentityUserRepository(
    BankAppDataContext dbContext,
    UserManager<IdentityUser> userManager
)
{
    public virtual async Task<IdentityUser?> CreateAsync(IdentityUser entity)
    {
        try
        {
            dbContext.Set<IdentityUser>().Add(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null!;
        }
    }

    public async Task<IdentityUser> GetAsync(string username, string hash)
    {
        try
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x =>
                x.UserName == username && x.PasswordHash == hash
            );
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<(IdentityUser? User, IdentityRole? Role)> GetAsync(
        Expression<Func<IdentityUser, bool>> expression
    )
    {
        try
        {
            var users = await dbContext.Users.ToListAsync();
            var userRoles = await dbContext.UserRoles.ToListAsync();
            var roles = await dbContext.Roles.ToListAsync();

            var result = (
                from user in users
                join userRole in userRoles on user.Id equals userRole.UserId
                join role in roles on userRole.RoleId equals role.Id
                select new { User = user, Role = role }
            ).FirstOrDefault(x => expression.Compile().Invoke(x.User));

            return (result?.User, result?.Role);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<(IdentityUser User, IdentityRole Role)>> GetAllAsync()
    {
        try
        {
            var result = await (
                from user in dbContext.Users
                join userRole in dbContext.UserRoles on user.Id equals userRole.UserId
                join role in dbContext.Roles on userRole.RoleId equals role.Id
                select new { User = user, Role = role }
            ).ToListAsync();

            return result.Select(x => (x.User, x.Role));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public virtual async Task<IdentityUser?> UpdateAsync(
        Expression<Func<IdentityUser, bool>> expression,
        IdentityUser entity
    )
    {
        try
        {
            var result = await dbContext.Set<IdentityUser>().FirstOrDefaultAsync(expression);
            if (result == null)
                return result!;

            dbContext.Entry(result).CurrentValues.SetValues(entity);
            await dbContext.SaveChangesAsync();

            return result!;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return null!;
    }

    public virtual async Task<bool> DeleteAsync(Expression<Func<IdentityUser, bool>> expression)
    {
        try
        {
            var result = await dbContext.Set<IdentityUser>().FirstOrDefaultAsync(expression);
            if (result == null)
                return false;

            dbContext.Set<IdentityUser>().Remove(result);
            await dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public virtual async Task<bool> ExistsByUserNameAsync(string username)
    {
        try
        {
            var result = await userManager.FindByNameAsync(username);
            return result == null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }

    public virtual async Task<bool> ExistsByEmailAsync(string email)
    {
        try
        {
            var result = await userManager.FindByEmailAsync(email);
            return result == null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }
}
