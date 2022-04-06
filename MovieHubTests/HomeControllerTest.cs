using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Controllers;
using MovieHub.Data;
using MovieHub.ViewModels;
using Xunit;
using Xunit.Abstractions;

namespace MovieHubTests;

public class HomeControllerTest
{
    private readonly HomeController _controller;

    public HomeControllerTest(ITestOutputHelper testOutputHelper)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("SurveyTestDatabase").Options;
        var context = new ApplicationDbContext(options);   
        _controller = new HomeController(context);
    }

    [Fact]
    public void Test_Should_Return_Index_View()
    {
        var result = _controller.Index();
        Assert.NotNull(result);
        Assert.IsType<Task<ActionResult<IndexViewModel>>>(result);
    }

    [Fact]
    public void Test_Return_Ok_On_Email_Input()
    {
        const string email = "test@testemail.com";
        var result = _controller.InsertEmail(email);
        Assert.NotNull(result);
    }
}