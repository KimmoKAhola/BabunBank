using System.Linq.Expressions;
using BabunBank.Data;
using BabunBank.Factories;
using BabunBank.Models;
using BabunBank.Repositories;

namespace BabunBank.Services;

public class UserService(UserRepository userRepository)
{
    public async Task<bool> CheckUserExistsAsync(Expression<Func<User, bool>> predicate)
    {
        try
        {
            return await userRepository.ExistsAsync(predicate);
        }
        catch (Exception e) { }

        return false;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        try
        {
            return await userRepository.GetAllAsync();
        }
        catch
        {
            return null!;
        }
    }

    public async Task<bool> CreateUserAsync(SignUpModel model)
    {
        //TODO add a check here. Need to implement a GUID or email to check if the user exists, not first name
        var user = UserFactory.Create(model);
        await userRepository.CreateAsync(user);
        return true;
    }
}