using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieHub.Controllers;
using MovieHub.Data;
using MovieHub.Models;
using MovieHub.ViewModels;
using Xunit;

namespace MovieHubTests;

public class AdminTests
{
    private readonly AdminController _controller;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("AdminTestDatabase").Options;
        _context = new ApplicationDbContext(options);
        _controller = new AdminController(_roleManager, _userManager);
    }

    [Fact]
    public void Test_Admin_Database_Ok()
    {
        _context.Database.EnsureCreated();
        InsertTestData(_context);
        
        // Counts all the roles in de TestDatabase (4)
        var sumOfRoles = _context.Roles.ToList().Count;
        
        // If sumOfRoles = 4, test is passed and Database works
        Assert.Equal(4, sumOfRoles);
    }
    
    private static void InsertTestData(DbContext context)
    {
        context.Add(new IdentityRole()
        {
            Name = "Admin",
            NormalizedName = "ADMIN"
        });

        context.Add(new IdentityRole()
        {
            Name = "Manager",
            NormalizedName = "MANAGER"
        });

        context.Add(new IdentityRole()
        {
            Name = "Front-Office",
            NormalizedName = "FRONT-OFFICE"
        });

        context.Add(new IdentityRole()
        {
            Name = "Back-Office",
            NormalizedName = "BACK-OFFICE"
        });
        
        
        
        context.SaveChanges();
    }
}