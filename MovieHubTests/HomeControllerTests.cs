using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using MovieHub.Controllers;
using MovieHub.Data;
using MovieHub.Models;
using Xunit;

namespace MovieHubTests;

public class HomeControllerTests : DatabaseTests
{
    private readonly HomeController _controller;

    public HomeControllerTests()
    {
        var logger = Mock.Of<ILogger<HomeController>>();
        _controller = new HomeController(logger, Context);
    }
    
    [Fact]
    public void Test_HomeController_MovieIndex_Should_Return_Todays_Movies()
    {
        var result = _controller.MovieIndex();
        Assert.Equal(DateTime.Today.Date, result.First().StartAt.Date); 
    }

    [Fact]
    public void Test_HomeController_GetMovies_Should_Return_Movies()
    {
        var result = _controller.GetMovies();
        Assert.IsType<Movie>(result.First());
    }
}