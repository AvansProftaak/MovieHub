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

public class PegisControllerTests
{
    private readonly PegisController _controller;
    private readonly ApplicationDbContext _context;
    
    public PegisControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("PegiTestDatabase").Options;
        _context = new ApplicationDbContext(options);
        _controller = new PegisController(_context);
    }
    
    [Fact]
    public void Test_Pegi_Database_Ok()
    {
        _context.Database.EnsureCreated();
        InsertTestData(_context);
        Assert.Equal("Pegi 16", _context.Pegi.First().Description);
    }
    
    [Fact]
    public void Test_Index_Returns_Index_View()
    {
        var result = _controller.Index;
        Assert.IsType<Func<Task<IActionResult>>>(result);
    }
    
    private void InsertTestData(ApplicationDbContext context)
    {
        context.Add(new Pegi()
        {
            Id = 1,
            Description = "Pegi 16",
            Icon = "https://static.wikia.nocookie.net/rating-system/images/c/cd/PGI4.png"
        });
        
        context.Add(new Pegi()
        {
            Id = 2,
            Description = "Violence",
            Icon = "https://static.wikia.nocookie.net/rating-system/images/e/e8/PEGI3.jpg"
        });
        
        context.Add(new Pegi()
        {
            Id = 3,
            Description = "Fear",
            Icon = "https://static.wikia.nocookie.net/rating-system/images/b/be/PEGI4.jpg"
        });
        
        context.Add(new Pegi()
        {
            Id = 4,
            Description = "Bad Language",
            Icon = "https://static.wikia.nocookie.net/rating-system/images/a/a5/PEGI1.jpg"
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

        context.Add(new MoviePegi()
        {
            PegiId = 1,
            MovieId = 1
        });
        
        context.Add(new MoviePegi()
        {
            PegiId = 2,
            MovieId = 1
        });
        
        context.Add(new MoviePegi()
        {
            PegiId = 3,
            MovieId = 1
        });
        
        context.Add(new MoviePegi()
        {
            PegiId = 4,
            MovieId = 1
        });
        
        context.SaveChanges();
    }
}