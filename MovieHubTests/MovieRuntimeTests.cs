using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MovieHub.Controllers;
using MovieHub.Data;
using MovieHub.Models;
using Xunit;

namespace MovieHubTests;

public class MovieRuntimeTests
{
    private readonly MovieRuntimesController _controller;
    private readonly ApplicationDbContext _context;

    public MovieRuntimeTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MovieRuntimesTestDatabase").Options;
        _context = new ApplicationDbContext(options);
        _controller = new MovieRuntimesController(_context);
    }
    
    
    
    

    private static MovieRuntime GetMovieRuntime()
    {
        return new MovieRuntime
        {
            Id = Guid.NewGuid().GetHashCode(), 
            HallId = 1, 
            MovieId = 2, 
            StartAt = new DateTime().Date.AddDays(22).AddMonths(03).AddYears(2022), 
            EndAt = new DateTime().Date.AddDays(12).AddMonths(04).AddYears(2022), 
            Time = TimeSpan.FromHours(12)
        };
    }
}