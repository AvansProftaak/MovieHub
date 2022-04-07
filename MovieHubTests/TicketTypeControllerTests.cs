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
        
        var ticket = _context.Tickettype
            .Where(t => t.Id.Equals(2)).ToList().FirstOrDefault();
        
        if (ticket == null)
        {
            Instantiate();
        }
    }

    internal void Instantiate()
    {
        _context.Database.EnsureCreated();
        InsertTestData(_context);
    }

    [Fact]
    public void Test_TicketType_Database_Ok()
    {

        Assert.IsType<Task<IActionResult>>(_controller.Index());
    }

    [Fact]
    public void Test_Details_Returns_TicketType_View()
    {
        var result = _controller.Details(1);
        Assert.IsType<Task<IActionResult>>(result);
    }

    [Fact]
    public async Task Throws_Error_When_Edit_Not_Right_Id()
    {
      
        var ticket =  _context.Tickettype
            .Where(t => t.Id.Equals(2)).ToList().FirstOrDefault();
        var result = await _controller.Edit(1 , ticket);

        Assert.IsType<NotFoundResult>(result);
    }
    
    
    [Fact]
    public void Test_Create_Action_Post_To_MemDB()
    {
        var ticketType = new Tickettype()
        {
            Id = 3,
            Name = "Test",
            Description = "For testing purposes",
            Price = 10,
            Quantity = 0
        };
            
        var result = _controller.Create(ticketType);
        
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
        
        context.Add( new Tickettype()
        {
            Id = 2,
            Name = "Test",
            Description = "For testing purposes",
            Price = 10,
            Quantity = 0
        });

        context.SaveChanges();
    }
    
    
}
