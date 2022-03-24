using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MovieHub.Controllers;
using MovieHub.Data;
using MovieHub.Models;
using Xunit;

namespace MovieHubTests;

public class HomeControllerTests
{
    private readonly HomeController _controller;
    private readonly ApplicationDbContext _context;
    
    public HomeControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("HomeTestDatabase").Options;
        _context = new ApplicationDbContext(options);   
        _controller = new HomeController(_context);
    }
    
    [Fact]
    public void Test_Home_Database_Ok()
    {
        _context.Database.EnsureCreated();
        InsertTestData(_context);
        Assert.Equal("Moviehub", _context.Cinema.First().Name);
    }
    
    [Fact]
    public void Test_Movie_Created()
    {
        Assert.Equal("Blacklight", _context.Movie.First().Title);
    }
    
    [Fact]
    public void Test_Movie_Runtime_Created()
    {
        Assert.Equal(1, _context.MovieRuntime.First().Id);
    }
    
    [Fact]
    public void Test_ShowTime_Created()
    {
        Assert.Equal(1, _context.Showtime?.First().Id);
    }
    
    // This test was commented out as it was only relevant in Kiosk mode retrieving today's movies.
    // [Fact] 
    // public void Test_HomeController_MovieIndex_Should_Return_Today's_Movies()
    // {
    //     var result = _controller.MovieIndex();
    //     Assert.Equal(DateTime.Today.Date, result.First().StartAt.Date); 
    // }

    [Fact]
    public void Test_HomeController_GetMovies_Should_Return_Movies()
    {
        var result = _controller.GetMovies();
        Assert.IsType<Movie>(result.First());
    }
    
    private static void InsertTestData(DbContext context)
    {
        context.Add(new Cinema()
        {
            Id = 1,
            Name = "Moviehub",
            Address = "Chasseveld 15",
            PostalCode = "4811 DH",
            City = "Breda",
            Country = "Nederland",
            Latitude = 51.58955,
            Longitude = 4.78544,
            FacebookUrl = "www.facebook.com/moviehub",
            InstagramUrl = "www.instagram.com/moviehub",
            TwitterUrl = "www.twitter.com/moviehub",
            YoutubeUrl = "www.youtu.be/moviehub"
        });

        context.Add(new Hall()
        {
            Id = 1,
            CinemaId = 1,
            Name = "Hall 1",
            Capacity = 120,
            Has3d = true,
            DolbySurround = true,
            WheelchairAccess = true
        });
        
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

        context.SaveChanges();
    }
}