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
using Xunit;

namespace MovieHubTests;

public class LostAndFoundControllerTest
{
    private readonly ApplicationDbContext _context;
    private readonly LostAndFoundController _controller;

    public LostAndFoundControllerTest()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("LostAndFoundTestDatabase").Options;
        _context = new ApplicationDbContext(options);
        _controller = new LostAndFoundController(_context);
    }
    
    [Fact]
    public async Task CreateShouldAddLostAndFoundToDbTest()
    {
        //arrange // Creates a new lostAndFound
        var lostAndFound = GetLostAndFound();
        
        // added lostAndFound to Database
        await _controller.Create(lostAndFound);
        
        //act // returns LostAndFound from Database with id
        var lostAndFounds = await _controller.GetLostAndFoundAsync();
        
        //assert// Check if data is same as the added lostAndFound
        lostAndFounds.First().Id.Should().Be(lostAndFound.Id);
        lostAndFounds.First().IssueDate.Should().Be(lostAndFound.IssueDate);
        lostAndFounds.First().Find.Should().Be(lostAndFound.Find);
        lostAndFounds.First().Description.Should().Be(lostAndFound.Description);
        lostAndFounds.First().Collected.Should().Be(lostAndFound.Collected);
        
        //act // returns LostAndFound from Database with id
        var createdlostAndFound = await _controller.GetLostAndFoundAsync(lostAndFound.Id);
        
        //assert// Check if data is same as the added lostAndFound with id
        createdlostAndFound.Id.Should().Be(lostAndFound.Id);
        createdlostAndFound.IssueDate.Should().Be(lostAndFound.IssueDate);
        createdlostAndFound.Find.Should().Be(lostAndFound.Find);
        createdlostAndFound.Description.Should().Be(lostAndFound.Description);
        createdlostAndFound.Collected.Should().Be(lostAndFound.Collected);
     
        await _context.Database.EnsureDeletedAsync();
    }
    [Fact]
    public async Task NotCollectedTest()
    {   
        // clear database
        await _context.Database.EnsureDeletedAsync();

        //arrange // Creates a new lostAndFound
        var lostAndFound = GetLostAndFound();
        
        // added lostAndFound to Database
        await _controller.Create(lostAndFound);
        
        //act // returns notCollected LostAndFounds from Database to indexView
        IActionResult actionResult = await _controller.NotCollected();

        //assert
        var viewResult = Assert.IsType<ViewResult>(actionResult);
        var model = Assert.IsAssignableFrom<IEnumerable<LostAndFound>>(
            viewResult.ViewData.Model);
        Assert.Equal(1, model.Count());

        await _context.Database.EnsureDeletedAsync();
    }
    
    [Fact]
    public async Task CleanListTest()
    {   
        // clear database
        await _context.Database.EnsureDeletedAsync();

        //arrange // Creates a new lostAndFound older then 30 days
        var lostAndFound = GetLostAndFoundOldDate();
        
        // added lostAndFound to Database
        await _controller.Create(lostAndFound);
        
        //act // removes LostAndFounds older then 30 days from Database
        _controller.CleanList();

        //assert// List count should be 0 because LostAndFoundOldDate is older then 30 days
        var lostAndFounds = await _controller.GetLostAndFoundAsync();
        Assert.Empty(lostAndFounds);
        
        await _context.Database.EnsureDeletedAsync();
    }
    
    [Fact]
    public async Task Test_Delete()
    {
        // clear database
        await _context.Database.EnsureDeletedAsync();

        //arrange // Creates a new lostAndFound
        var lostAndFound = GetLostAndFound();
        
        // added lostAndFound to Database
        await _controller.Create(lostAndFound);
        var lostAndFounds = await _controller.GetLostAndFoundAsync();
        
        //assert// Check if data is same as the added lostAndFound
        lostAndFounds.First().Id.Should().Be(lostAndFound.Id);
        lostAndFounds.First().IssueDate.Should().Be(lostAndFound.IssueDate);
        lostAndFounds.First().Find.Should().Be(lostAndFound.Find);
        lostAndFounds.First().Description.Should().Be(lostAndFound.Description);
        lostAndFounds.First().Collected.Should().Be(lostAndFound.Collected);
        
        //Assert 1 item in list
        lostAndFounds.Count.Should().Be(1);
        
        //act // returns LostAndFound from Database with id
        var createdlostAndFound = await _controller.GetLostAndFoundAsync(lostAndFound.Id);
        
        //assert// Check if data is same as the added lostAndFound with id
        createdlostAndFound.Id.Should().Be(lostAndFound.Id);
        createdlostAndFound.IssueDate.Should().Be(lostAndFound.IssueDate);
        createdlostAndFound.Find.Should().Be(lostAndFound.Find);
        createdlostAndFound.Description.Should().Be(lostAndFound.Description);
        createdlostAndFound.Collected.Should().Be(lostAndFound.Collected);

        //delete and return list
        await _controller.DeleteConfirmed(lostAndFound.Id);
        lostAndFounds = await _controller.GetLostAndFoundAsync();
        
        //Now list is empty
        lostAndFounds.Count.Should().Be(0);
        
        await _context.Database.EnsureDeletedAsync();
    }
    
    private static LostAndFound GetLostAndFound()
    {
        return new LostAndFound
        {
            Id = Guid.NewGuid().GetHashCode(), 
            IssueDate = DateTime.Now,
            Find = "cloakroom",
            Description = "umbrella",
            Collected = false
        };
    }
    private static LostAndFound GetLostAndFoundOldDate()
    {
        return new LostAndFound
        {
            Id = Guid.NewGuid().GetHashCode(), 
            IssueDate = DateTime.Now.AddDays(-50),
            Find = "cloakroom",
            Description = "umbrella",
            Collected = true
        };
    }
}