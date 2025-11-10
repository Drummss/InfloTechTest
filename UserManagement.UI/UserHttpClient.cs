using System.Net.Http.Json;
using UserManagement.UI.DTOs;
using UserManagement.UI.Models;

namespace UserManagement.UI;

public class UserHttpClient(HttpClient client)
{
    public async ValueTask<IEnumerable<User>> GetUsersAsync(bool? isActive = null)
    {
        if (isActive.HasValue)
        {
            return await client.GetFromJsonAsync<IEnumerable<User>>($"api/users?isActive={isActive.Value}") ?? [];
        }

        return await client.GetFromJsonAsync<IEnumerable<User>>($"api/users") ?? [];
    }

    public async ValueTask<User?> GetUserByIdAsync(long userId)
    {
        return await client.GetFromJsonAsync<User>($"api/users/{userId}");
    }

    public async ValueTask CreateUserAsync(CreateUserRequestModel user)
    {
        var response = await client.PostAsJsonAsync("api/users", user);
        response.EnsureSuccessStatusCode();
    }

    public async ValueTask UpdateUserAsync(long userId, UpdateUserRequestModel user)
    {
        var response = await client.PutAsJsonAsync($"api/users/{userId}", user);
        response.EnsureSuccessStatusCode();
    }

    public async ValueTask DeleteUserAsync(long userId)
    {
        var response = await client.DeleteAsync($"api/users/{userId}");
        response.EnsureSuccessStatusCode();
    }
}
