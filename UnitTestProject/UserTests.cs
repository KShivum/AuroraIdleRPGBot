using AuroraAPi;
using AuroraAPi.Controllers;
using AuroraLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace UnitTestProject;

public class UserTests : IDisposable
{
    SqliteConnection _connection;
    private AuroraContext _context;
    public UserTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var contextOptions = new DbContextOptionsBuilder<AuroraContext>().UseSqlite(_connection).Options;
        _context = new AuroraContext(contextOptions);
        
        _context.Database.EnsureCreated();
        _context.AddRange(
            new User {Name = "Bob", XP = 43, DiscordSnowflake = 18446744073709551615},
            new User {Name = "Joe", XP = 0, DiscordSnowflake = 18446744073705357312},
            new User {Name = "Sally", XP = 100});
        _context.SaveChanges();
    }
    
    [Fact]
    public async Task TestGetUsers()
    {
        var controller = new UserController(_context);
        var result = await controller.GetUsers();
        Assert.Equal(3, result.Value.Count());
    }
    
    [Fact]
    public async Task TestGetUser()
    {
        var controller = new UserController(_context);
        var result = await controller.GetUser(1);
        Assert.Equal("Bob", result.Value.Name);
    }
    
    
    [Fact]
    public async Task TestPostUserDiscordSnowFlake()
    {
        var controller = new UserController(_context);
        var result = await controller.PostUser(new User {Name = "Steven", XP = 43, DiscordSnowflake = 18446744073709551615});
        Assert.IsType<CreatedAtActionResult>(result.Result);
        var user = await controller.GetUser(4);
        Assert.Equal("Steven", user.Value.Name);
    }

    [Fact]
    public async Task TestPostUserNoDiscordSnowFlake()
    {
        var controller = new UserController(_context);
        var result = await controller.PostUser(new User {Name = "Steven", XP = 43});
        Assert.IsType<CreatedAtActionResult>(result.Result);
        var user = await controller.GetUser(4);
        Assert.Equal("Steven", user.Value.Name);
    }
    
    [Fact]
    public async Task TestDeleteUser()
    {
        var controller = new UserController(_context);
        var result = await controller.DeleteUser(1);
        Assert.IsType<NoContentResult>(result);
        var user = await controller.GetUser(1);
        Assert.IsType<NotFoundResult>(user.Result);
    }
    
    
    public void Dispose()
    {
     _connection.Close();   
    }
}