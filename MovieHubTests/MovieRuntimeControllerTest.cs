using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Controllers;
using MovieHub.Data;
using MovieHub.Models;
using MovieHub.ViewModels;
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
    
    
    // Tests if index-page is loaded ans shows all Halls 
    [Fact]
    public void Test_Index()
    {
        var result = _controller.Index();
        
        // Checks if it returns Index page
        Assert.IsType<Task<IActionResult>>(result);
    }
    
    [Fact]
    public async Task Test_Create()
    {
        // delete database for a fresh start
        await _context.Database.EnsureDeletedAsync();
        
        // Creates new MovieRuntime without Time 
        var movieRuntime = GetMovieRuntimeWithoutTime();
        
        // Added time (like in controller)
        var time = new TimeSpan(12, 0, 0);
        
        // Adds MovieRuntime to database
        await _controller.Create(movieRuntime, time);
        
        // Get the MovieRuntime from database with id as movieRuntimeId
        var createdMovieRuntime = await GetOneMovieRuntimeTestAsync(movieRuntime.Id);
        
        // Checks if data is same
        createdMovieRuntime.Id.Should().Be(movieRuntime.Id);
        createdMovieRuntime.HallId.Should().Be(movieRuntime.HallId);

        // Gets a list with all showtimes from database
        var createdShowtimes = await GetShowtimesTestAsync();
        createdMovieRuntime.Id.Should().Be(movieRuntime.Id);
        createdMovieRuntime.StartAt.Should().Be(movieRuntime.StartAt);

        // We've added the MovieRuntime to play at 4 days: Today + 3 days
        // There has to be a Showtime list with 4 a length of 4 (Showtimes)
        createdShowtimes.Count.Should().Be(4);
    }

    [Fact]
    public async Task Test_Edit()
    {
        // delete database for a fresh start
        await _context.Database.EnsureDeletedAsync();
        
        // Adds MovieRuntime to database
        var movieRuntime = GetMovieRuntimeWithoutTime();
        var time = new TimeSpan(12, 0, 0);
        await _controller.Create(movieRuntime, time);
        
        // Get the MovieRuntime from database with id as movieRuntimeId
        var runtimeList = await GetAllMovieRuntimesTestAsync();
        runtimeList.First().StartAt.Should().Be(movieRuntime.StartAt);
        runtimeList.First().Time.Should().Be(time);
        
        // Edit the movieRuntime
        var editedMovieRuntime = GetMovieRuntimeWithoutTime2();
        var editedTime = new TimeSpan(20, 0, 0);
        await _controller.Edit(runtimeList.First().Id, editedMovieRuntime, editedTime);
        
        // Gets a new RuntimeList from database
        runtimeList = await GetAllMovieRuntimesTestAsync();
        runtimeList.First().StartAt.Should().Be(editedMovieRuntime.StartAt);
        runtimeList.First().Time.Should().Be(editedTime);
    }
    
    [Fact]
    public async Task Test_Delete()
    {
        // delete database for a fresh start
        await _context.Database.EnsureDeletedAsync();
        
        // Adds 2 MovieRuntimes to database
        var movieRuntime = GetMovieRuntimeWithoutTime();
        var movieRuntime2 = GetMovieRuntimeWithoutTime2();
        var time = new TimeSpan(12, 0, 0);
        await _controller.Create(movieRuntime, time);
        await _controller.Create(movieRuntime2, time);
        
        // Get the MovieRuntime from database with id as movieRuntimeId
        // Gets the created Showtimes
        var runtimeList = await GetAllMovieRuntimesTestAsync();
        var showtimeList = await GetShowtimesTestAsync();
        
        // Checks if RuntimeList contains 1 item
        // Checks if ShowtimeList contains 4 items
        runtimeList.Count.Should().Be(2);
        showtimeList.Count.Should().Be(8);

        // Deletes first (and only) item from list
        await _controller.DeleteConfirmed(runtimeList[0].Id);
        
        // Gets the RuntimeList and ShowtimeList again
        runtimeList = await GetAllMovieRuntimesTestAsync();
        showtimeList = await GetShowtimesTestAsync();

        // Checks if RuntimeList has 1 item (1 was deleted)
        // Checks if ShowtimeList contains 4 items (4 were deleted)
        runtimeList.Count.Should().Be(1);
        showtimeList.Count.Should().Be(4);
    }

    
    
    // TEST Functions
    private async Task<MovieRuntime> GetOneMovieRuntimeTestAsync(int id)
    {
        return await _context.MovieRuntime
            .Where(m => m.Id == id)
            .OrderBy(m => m.Time)
            .FirstAsync();
    }
    
    public async Task<List<Showtime>> GetShowtimesTestAsync()
    {
        return await _context.Showtime.ToListAsync();
    }
    
    public async Task<IList<MovieRuntime>> GetAllMovieRuntimesTestAsync()
    {
        return await _context.MovieRuntime
            .OrderBy(m => m.Time)
            .ToListAsync();
    }
    
    // OBJECTS:
    private static MovieRuntime GetMovieRuntimeWithoutTime()
    {
        return new MovieRuntime
        {
            Id = 1,
            MovieId = 1,
            HallId = 2,
            StartAt = DateTime.Today.AddDays(3).Date,
            EndAt = DateTime.Today.AddDays(6).Date,
            Time = TimeSpan.FromHours(12)
        };
    }
    
    private static MovieRuntime GetMovieRuntimeWithoutTime2()
    {
        return new MovieRuntime
        {
            Id = 1,
            MovieId = 1,
            HallId = 2,
            StartAt = DateTime.Today.Date,
            EndAt = DateTime.Today.AddDays(3).Date,
            Time = TimeSpan.FromHours(12)
        };
    }

    private static Hall GetHall()
    {
        return new Hall
        {
            Id = 1,
            Name = "CINEMA 2",
            Capacity = 150,
            Has3d = true,
            DolbySurround = true,
            WheelchairAccess = true
        };
    }
}