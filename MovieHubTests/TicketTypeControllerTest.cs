using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Controllers;
using MovieHub.Data;
using MovieHub.Models;
using Xunit;

namespace MovieHubTests;

public class TicketTypeControllerTest
{
    private readonly ApplicationDbContext _context;
    private readonly TicketTypeController _controller;

    public TicketTypeControllerTest()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TicketTypeControllerTestDatabase").Options;
        _context = new ApplicationDbContext(options);
        _controller = new TicketTypeController(_context);
    }
    [Fact]
    public void TestTicketTypeDatabaseOk()
    {
        // clear database
        _context.Database.EnsureDeletedAsync();
        //arrange // Creates a new ticketType
        var ticketType = AddNormalTickettype();
        
        // added ticketType to Database
       _controller.Create(ticketType);
        
        Assert.IsType<Task<IActionResult>>(_controller.Index());
    }
    
    [Fact]
    public async Task CreateShouldAddTicketTypeToDbTest()
    {
        // clear database
        await _context.Database.EnsureDeletedAsync();
        //arrange // Creates a new ticketType
        var ticketType = AddNormalTickettype();
        
        // added ticketType to Database
        await _controller.Create(ticketType);
        
        //act // returns ticketType from Database with id
        var ticketTypes = await _controller.GetTicketTypeAsync();
        
        //assert// Check if data is same as the added ticketType
        ticketTypes.First().Id.Should().Be(ticketType.Id);
        ticketTypes.First().Name.Should().Be(ticketType.Name);
        ticketTypes.First().Description.Should().Be(ticketType.Description);
        ticketTypes.First().Price.Should().Be(ticketType.Price);
        
        //act // returns ticketType from Database with id
        var createdTickettype = await _controller.GetTicketTypeAsync(ticketType.Id);
        
        //assert// Check if data is same as the added ticketType with id
        createdTickettype.Id.Should().Be(ticketType.Id);
        createdTickettype.Name.Should().Be(ticketType.Name);
        createdTickettype.Description.Should().Be(ticketType.Description);
        createdTickettype.Price.Should().Be(ticketType.Price);

        await _context.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async Task TickettypeExistsTest()
    {
        // clear database
        await _context.Database.EnsureDeletedAsync();

        //arrange // Creates a new ticketType
        var ticketType = AddNormalTickettype();

        // added ticketType to Database
        await _controller.Create(ticketType);

        //act // returns boolean ticketType Exists with id
        var result = _controller.TickettypeExists(ticketType.Id);
        Assert.True(result);
        
    }
    
    private static Tickettype AddNormalTickettype()
    {
        return new Tickettype
        {
            Id = Guid.NewGuid().GetHashCode(),
            Name = "Normal",
            Price = 10,
            Description = "Normal ticket",
        };
    }
}