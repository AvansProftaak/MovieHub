using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Controllers;
using MovieHub.Data;
using MovieHub.Models;
using Xunit;

namespace MovieHubTests;

public class MoviesControllerTests
{
    private readonly MoviesController _controller;
    private readonly ApplicationDbContext _context;

    public MoviesControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MovieTestDatabase").Options;
        _context = new ApplicationDbContext(options);
        _controller = new MoviesController(_context);
    }
    
    [Fact]
    public void Test_Movies_Database_Ok()
    {
        _context.Database.EnsureCreated();
        InsertTestData(_context);
        Assert.Equal("Blacklight", _context.Movie.First().Title);
    }

    [Fact]
    public void Test_Details_Returns_Details_View()
    {
        var result = _controller.Details(1);
        Assert.IsType<Task<IActionResult>>(result);
    }

    private void InsertTestData(ApplicationDbContext context)
    {
        context.Add(new Movie()
        {
            Id = 1,
            Title = "Blacklight",
            Description = "This is the description for Blacklight",
            Duration = 120,
            Cast = "Brad Pitt",
            Director = "Steven Spielberg",
            ImdbScore = 7.0,
            ReleaseDate = DateTime.Now.AddDays(-7),
            Is3D = false,
            IsSecret = false,
            Language = "English",
            ImageUrl = "https://i.ibb.co/YyqyK3S/blacklight.jpg",
            TrailerUrl = "https://www.youtube.com/watch?v=PE04ESdgnHI"
        });
        
        context.Add(new MovieRuntime()
        {
            Id = 1,
            MovieId = 1,
            HallId = 1,
            StartAt = DateTime.Today.Date,
            EndAt = DateTime.Today.AddDays(1).Date,
            Time = TimeSpan.FromHours(23)
        });

        context.Add(new Showtime()
        {
            HallId = 1,
            MovieId = 1,
            StartAt = DateTime.Today.AddHours(23)
        });
        
        context.Add(new Showtime()
        {
            HallId = 1,
            MovieId = 1,
            StartAt = DateTime.Today.AddDays(1).AddHours(23)
        });

        context.Add(new Pegi()
        {
            Id = 1,
            Description = "Gambling",
            Icon = "https://static.wikia.nocookie.net/rating-system/images/4/40/PEGI6.jpg"
        });
        
        context.Add(new Pegi()
        {
            Id = 2,
            Description = "Violence",
            Icon = "https://static.wikia.nocookie.net/rating-system/images/e/e8/PEGI3.jpg"
        });

        context.Add(new MoviePegi()
        {
            MovieId = 1,
            PegiId = 1
        });
        
        context.Add(new MoviePegi()
        {
            MovieId = 1,
            PegiId = 2
        });

        context.Add(new Genre()
        {
            GenreEnum = GenreEnum.Action
        });
        
        context.Add(new Genre()
        {
            GenreEnum = GenreEnum.Adventure
        });

        context.Add(new MovieGenre()
        {
            MovieId = 1,
            GenreId = 1
        });
        
        context.Add(new MovieGenre()
        {
            MovieId = 1,
            GenreId = 2
        });
        
        context.SaveChanges();
    }
}