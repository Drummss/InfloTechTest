using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Web.Tests;

public class UserControllerTests
{
    private readonly Mock<IUserService> _userService;

    private readonly List<User> _users =
    [
        new()
        {
            Id = 1,
            Forename = "Johnny",
            Surname = "User",
            Email = "juser@example.com",
            DateOfBirth = new DateTime(1996, 12, 24),
            IsActive = true,
        },
        new()
        {
            Id = 2,
            Forename = "Sam",
            Surname = "User",
            Email = "suser@example.com",
            DateOfBirth = new DateTime(1999, 4, 3),
            IsActive = true,
        },
        new()
        {
            Id = 3,
            Forename = "Katie",
            Surname = "User",
            Email = "kuser@example.com",
            IsActive = false,
        },
    ];

    public UserControllerTests()
    {
        _userService = new Mock<IUserService>();

        _userService
            .Setup(s => s.GetAll())
            .Returns(_users);

        _userService
            .Setup(s => s.FilterByActive(true))
            .Returns(_users.Where(u => u.IsActive));

        _userService
            .Setup(s => s.FilterByActive(false))
            .Returns(_users.Where(u => !u.IsActive));
    }

    [Fact]
    public void ListModelContainsUsersTest()
    {
        var controller = CreateController();
        var result = controller.List();

        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().BeEquivalentTo(_users);
    }

    [Fact]
    public void ListModelFiltersByActiveTest()
    {
        var controller = CreateController();
        var result = controller.List(true);

        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().HaveCount(2);
    }

    [Fact]
    public void ListModelFiltersByInactiveTest()
    {
        var controller = CreateController();
        var result = controller.List(false);

        result.Model.Should().BeOfType<UserListViewModel>().Which.Items.Should().HaveCount(1);
    }

    private UsersController CreateController() => new(_userService.Object);
}
