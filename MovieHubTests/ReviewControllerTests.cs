using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using MovieHub.Controllers;
using MovieHub.Data;
using MovieHub.Models;
using Xunit;

namespace MovieHubTests;

public class ReviewControllerTests
{
    private readonly ReviewController _controller;
    private readonly ApplicationDbContext _context;

    public ReviewControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MovieTestDatabase").Options;
        _context = new ApplicationDbContext(options);
        _controller = new ReviewController(_context);
    }
    
    [Fact]
    public void Test_Review_Database_Ok()
    {
        _context.Database.EnsureCreated();
        InsertTestData(_context);
        Assert.Equal("Bart", _context.Review.First().Name);
    }

    [Fact]
    public void Test_Details_Returns_Details_View()
    {
        var result = _controller.Details(1);
        Assert.IsType<Task<IActionResult>>(result);
    }

    private void InsertTestData(ApplicationDbContext context)
    {
        context.Add(new Review()
        {
            Id = 1,
            CinemaId = 1,
            HallId = 2,
            DisplayQuality = 4,
            SoundQuality = 4,
            Disturbance = 2,
            FoodQuality = 4,
            Hygiene = 5,
            Email = "bart-grootoonk@hotmail.com",
            Name = "Bart"

        });

        context.SaveChanges();
    }
}
    