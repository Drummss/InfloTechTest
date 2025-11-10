using Microsoft.AspNetCore.Mvc;
using UserManagement.Api.Services;
using UserManagement.Models;

namespace UserManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(UserService userService)
{
    [HttpGet]
    public async ValueTask<IResult> GetUsers(bool? isActive)
    {
        var users = isActive.HasValue
            ? await userService.GetUsersFilteredByActiveAsync(isActive.Value)
            : await userService.GetUsersAsync();

        return Results.Json(users);
    }

    [HttpGet("{id}")]
    public async ValueTask<IResult> GetUserById(long id)
    {
        var user = await userService.GetUserByIdAsync(id);

        if (user == null)
        {
            return Results.NotFound();
        }

        return Results.Json(user);
    }

    [HttpPost]
    public async ValueTask<IResult> CreateUser([FromBody] User userModel)
    {
        User newUser = await userService.CreateUserAsync(userModel);

        return Results.Created($"/api/users/{userModel.Id}", newUser);
    }

    [HttpPut("{id}")]
    public async ValueTask<IResult> UpdateUser(long id, [FromBody] User userModel)
    {
        var existingUser = await userService.GetUserByIdAsync(id);
        if (existingUser == null)
        {
            return Results.NotFound();
        }

        await userService.UpdateUserAsync(id, userModel);
        return Results.Ok();
    }

    [HttpDelete("{id}")]
    public async ValueTask<IResult> DeleteUser(long id)
    {
        var existingUser = await userService.GetUserByIdAsync(id);
        if (existingUser == null)
        {
            return Results.NotFound();
        }

        await userService.DeleteUserAsync(id);
        return Results.NoContent();
    }
}
