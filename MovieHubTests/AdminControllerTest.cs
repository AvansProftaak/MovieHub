using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Controllers;
using MovieHub.Data;
using MovieHub.Models;
using MovieHub.ViewModels;
using Xunit;
using Xunit.Abstractions;

namespace MovieHubTests;

public class AdminTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly AdminController _controller;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("AdminTestDatabase").Options;
        _context = new ApplicationDbContext(options);
        _controller = new AdminController(_roleManager, _userManager);
    }

    [Fact]
    public void Test_Should_Return_Index_View()
    {
        var result = _controller.RoleList();
        Assert.IsType<Task<IActionResult>>(result);
    }
    
    private static IEnumerable<IdentityRole> GetRoles()
    {
        return new List<IdentityRole>
        {
            new()
            {
                Id = "ID-1",
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new()
            {
                Id = "ID-2",
                Name = "Manager",
                NormalizedName = "MANAGER"
            },
            new(){
                Id = "ID-3",
                Name = "Front-Office",
                NormalizedName = "FRONT-OFFICE"
            },
            new(){
                Id = "ID-4",
                Name = "Back-Office",
                NormalizedName = "BACK-OFFICE"
            }
        };
    }
}