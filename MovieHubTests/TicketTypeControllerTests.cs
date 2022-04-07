using System;
using System.Collections.Generic;
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

public class TicketTypeControllerTests
{
    private readonly TicketTypeController _controller;
    private readonly ApplicationDbContext _context;

    public TicketTypeControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MovieTestDatabase").Options;
        _context = new ApplicationDbContext(options);
        _controller = new TicketTypeController(_context);
    }

    [Fact]
    public void Test_TicketType_Database_Ok()
    {
        _context.Database.EnsureCreated();
        InsertTestData(_context);
        
        Assert.IsType<Task<IActionResult>>(_controller.Index());
    }

    [Fact]
    public void Test_Details_Returns_TicketType_View()
    {
        var result = _controller.Details(1);
        Assert.IsType<Task<IActionResult>>(result);
    }

    private void InsertTestData(ApplicationDbContext context)
    {
        context.Add(new Tickettype()
        {
            Id = 1,
            Name = "TestType",
            Description = "For testing purposes",
            Price = 10,
            Quantity = 0,
        });

        context.SaveChanges();
    }
}
