using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Data.Patterns;
using UserManagement.Models;

namespace UserManagement.Api.Services;

public class UserService(DbContext dbContext, IEntityRepository<User> userRepository)
{
    public async ValueTask<IEnumerable<User>> GetUsersAsync()
    {
        return await userRepository.AllAsync();
    }

    public async ValueTask<IEnumerable<User>> GetUsersFilteredByActiveAsync(bool isActive)
    {
        return (await userRepository.AllAsync()).Where(u => u.IsActive == isActive);
    }

    public async ValueTask<User?> GetUserByIdAsync(long id)
    {
        return (await userRepository.AllAsync()).FirstOrDefault(x => x.Id == id);
    }

    public async ValueTask<User> CreateUserAsync(User user)
    {
        User newUser = await userRepository.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return newUser;
    }

    public async ValueTask<User> UpdateUserAsync(long id, User updatedUser)
    {
        var existingUser = (await userRepository.AllAsync()).FirstOrDefault(u => u.Id == id);

        if (existingUser == null)
        {
            throw new ArgumentException($"User with ID {id} not found.");
        }

        existingUser.Forename = updatedUser.Forename;
        existingUser.Surname = updatedUser.Surname;
        existingUser.Email = updatedUser.Email;
        existingUser.DateOfBirth = updatedUser.DateOfBirth;
        existingUser.IsActive = updatedUser.IsActive;

        await userRepository.UpdateAsync(existingUser);
        await dbContext.SaveChangesAsync();

        return existingUser;
    }

    public async ValueTask DeleteUserAsync(long id)
    {
        var existingUser = (await userRepository.AllAsync()).FirstOrDefault(u => u.Id == id);

        if (existingUser == null)
        {
            throw new ArgumentException($"User with ID {id} not found.");
        }

        await userRepository.DeleteAsync(existingUser);
        await dbContext.SaveChangesAsync();
    }
}
