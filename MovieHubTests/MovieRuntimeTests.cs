using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MovieHub.Controllers;
using MovieHub.Data;
using MovieHub.Models;
using Xunit;
using Xunit.Abstractions;

namespace MovieHubTests;

public class MovieRuntimeTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly MovieRuntimesController _controller;

    public MovieRuntimeTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MovieRuntimesTestDatabase").Options;
        var context = new ApplicationDbContext(options);
        _controller = new MovieRuntimesController(context);
    }
    
    [Fact]
    public async Task Test_Create_MovieRuntime_With_ShowTimes()
    {
        var movieRuntime = GetMovieRuntimeWithoutTime();
        var time = new TimeSpan(12, 0, 0);
        await _controller.CreateMovieRuntimeAsync(movieRuntime, time);
        
        var movieRuntimes = await _controller.GetMovieRuntimesAsync();
        
        _testOutputHelper.WriteLine(movieRuntimes[0].Time.ToString());
        
        movieRuntimes.Single().HallId.Should().Be(movieRuntime.HallId);
        movieRuntimes.Single().StartAt.Should().Be(movieRuntime.StartAt);
        movieRuntimes.Single().Id.Should().Be(movieRuntime.Id);
        movieRuntimes.Single().Time.Should().Be(movieRuntime.Time);
        
        // var createdMovieRuntime = await _controller.GetOneMovieRuntimeAsync(movieRuntime.Id);


    }
    
    private static MovieRuntime GetMovieRuntimeWithoutTime()
    {
        return new MovieRuntime
        {
            Id = 1,
            MovieId = 1,
            HallId = 2,
            StartAt = DateTime.Today.Date,
            EndAt = DateTime.Today.AddDays(1).Date,
            Time = TimeSpan.FromHours(12)
        };
    }
}