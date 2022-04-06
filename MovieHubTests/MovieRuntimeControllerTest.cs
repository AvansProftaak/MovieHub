using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MovieHub.Controllers;
using MovieHub.Data;
using MovieHub.Models;
using NuGet.Protocol;
using Xunit;
using Xunit.Abstractions;

namespace MovieHubTests;

public class MovieRuntimeControllerTest
{
    private readonly MovieRuntimesController _controller;
    private readonly ApplicationDbContext _context;

    public MovieRuntimeControllerTest()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MovieRuntimesTestDatabase").Options;
        _context = new ApplicationDbContext(options);
        _controller = new MovieRuntimesController(_context);
    }
    
    [Fact]
    public async Task Test_Create_MovieRuntime_With_ShowTimes()
    {
        // Creates new MovieRuntime without Time 
        var movieRuntime = GetMovieRuntimeWithoutTime();
        
        // Added time (like in controller)
        var time = new TimeSpan(12, 0, 0);
        
        // Adds MovieRuntime to Database
        await _controller.CreateMovieRuntimeAsync(movieRuntime, time);
        
        // Gets List with all MovieRuntimes from database
        var movieRuntimes = await _controller.GetMovieRuntimesAsync();
        
        // Test if all callbacks are right
        movieRuntimes.First().HallId.Should().Be(movieRuntime.HallId);
        movieRuntimes.First().StartAt.Should().Be(movieRuntime.StartAt);
        movieRuntimes.First().Id.Should().Be(movieRuntime.Id);
        movieRuntimes.First().Time.Should().Be(movieRuntime.Time);
        

    }

    [Fact]
    public async Task Test_Get_One_MovieRuntime_From_Db()
    {
        // Creates new MovieRuntime without Time 
        var movieRuntime = GetMovieRuntimeWithoutTime();
        
        // Added time (like in controller)
        var time = new TimeSpan(12, 0, 0);
        
        // Adds MovieRuntime to Database
        await _controller.CreateMovieRuntimeAsync(movieRuntime, time);
        
        // Get the MovieRuntime from Database with id as movieRuntimeId
        var createdMovieRuntime = await _controller.GetOneMovieRuntimeTestAsync(movieRuntime.Id);
        
        // Checks if data is same
        createdMovieRuntime.Id.Should().Be(movieRuntime.Id);
        createdMovieRuntime.HallId.Should().Be(movieRuntime.HallId);
        
    }


    
    // OBJECTS:
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