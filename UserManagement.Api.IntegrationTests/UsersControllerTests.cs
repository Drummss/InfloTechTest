using System.Net;
using FluentAssertions;
using UserManagement.Api.IntegrationTests.Core;

namespace UserManagement.Api.IntegrationTests;

public class UsersControllerTests(ContainerEnvironment containerEnvironment) : ContainerTestBase(containerEnvironment)
{
    [Fact]
    public async Task GetUsers_ReturnsOk()
    {
        HttpClient client = CreateDefaultClient();

        HttpResponseMessage response = await client.GetAsync("/api/users");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateUser_ReturnsSuccess_WhenValid()
    {
        HttpClient client = CreateDefaultClient();

        var newUser = new
        {
            Forename = "John",
            Surname = "Doe",
            Email = "jdoe@example.com",
            DateOfBirth = "1990-01-01",
            IsActive = true
        };

        HttpResponseMessage response = await client.PostAsJsonAsync("/api/users", newUser);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task GetUserById_ReturnsUser_WhenExists()
    {
        HttpClient client = CreateClient();

        var newUser = new
        {
            Forename = "Jane",
            Surname = "Doe",
            Email = "jdoe@example.com",
            DateOfBirth = "1992-02-02",
            IsActive = true
        };

        var createResponse = await client.PostAsJsonAsync("/api/users", newUser);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserModel>();

        var response = await client.GetAsync($"/api/users/{createdUser!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = await response.Content.ReadFromJsonAsync<UserModel>();

        user.Should().NotBeNull();
        user.Forename.Should().Be("Jane");
    }

    [Fact]
    public async Task GetUserById_ReturnsNotFound_WhenNotExists()
    {
        HttpClient client = CreateClient();

        var response = await client.GetAsync($"/api/users/999999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateUser_ReturnsNoContent_WhenExists()
    {
        HttpClient client = CreateClient();

        var newUser = new
        {
            Forename = "Update",
            Surname = "Me",
            Email = "update@example.com",
            DateOfBirth = "1995-05-05",
            IsActive = true
        };

        var createResponse = await client.PostAsJsonAsync("/api/users", newUser);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserModel>();

        createdUser.Should().NotBeNull();
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var updatedUser = new
        {
            createdUser!.Id,
            Forename = "Updated",
            Surname = "Name",
            Email = "updated@example.com",
            DateOfBirth = "1995-05-05",
            IsActive = false
        };

        var response = await client.PutAsJsonAsync($"/api/users/{createdUser.Id}", updatedUser);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedUserResponse = await client.GetAsync($"/api/users/{createdUser.Id}");
        var user = await updatedUserResponse.Content.ReadFromJsonAsync<UserModel>();

        updatedUserResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        user.Should().NotBeNull();
        user.Forename.Should().Be("Updated");
    }

    [Fact]
    public async Task UpdateUser_ReturnsNotFound_WhenNotExists()
    {
        HttpClient client = CreateClient();

        var updatedUser = new
        {
            Forename = "Not",
            Surname = "Found",
            Email = "notfound@example.com",
            DateOfBirth = "2000-01-01",
            IsActive = false
        };

        var response = await client.PutAsJsonAsync($"/api/users/999", updatedUser);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteUser_ReturnsNoContent_WhenExists()
    {
        HttpClient client = CreateClient();

        var newUser = new
        {
            Forename = "Deleted",
            Surname = "User",
            Email = "deleteduser@example.com",
            DateOfBirth = "1993-03-03",
            IsActive = true
        };

        var createResponse = await client.PostAsJsonAsync("/api/users", newUser);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserModel>();

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        createdUser.Should().NotBeNull();

        var response = await client.DeleteAsync($"/api/users/{createdUser!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteUser_ReturnsNotFound_WhenNotExists()
    {
        HttpClient client = CreateClient();

        var response = await client.DeleteAsync($"/api/users/999");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private class UserModel
    {
        public long Id { get; set; }
        public required string Forename { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public required bool IsActive { get; set; }
    }
}
