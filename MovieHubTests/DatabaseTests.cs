using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;
using MovieHub.Models;
using Xunit;

namespace MovieHubTests;

public abstract class DatabaseTests
{
    protected readonly ApplicationDbContext Context;
    
    protected DatabaseTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("test_db").Options;
        Context = new ApplicationDbContext(options);    
    }
    
    [Fact]
    public void Test_Database_Ok()
    {
        Context.Database.EnsureCreated();
        InsertTestData(Context);
        Assert.Equal("Moviehub", Context.Cinema.First().Name);
    }
    
    [Fact]
    public void Test_Movie_Created()
    {
        Assert.Equal("Blacklight", Context.Movie.First().Title);
    }
    
    [Fact]
    public void Test_Movie_Runtime_Created()
    {
        Assert.Equal(1, Context.MovieRuntime.First().Id);
    }
    
    [Fact]
    public void Test_ShowTime_Created()
    {
        Assert.Equal(1, Context.Showtime.First().Id);
    }
    private void InsertTestData(ApplicationDbContext context)
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
            Time = TimeSpan.FromHours(19)
        });

        context.Add(new Showtime()
        {
            HallId = 1,
            MovieId = 1,
            StartAt = DateTime.Today.AddHours(19)
        });
        
        context.Add(new Showtime()
        {
            HallId = 1,
            MovieId = 1,
            StartAt = DateTime.Today.AddDays(1).AddHours(19)
        });

        context.SaveChanges();
    }
}