using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub;
using MovieHub.Data;
using MovieHub.Models;
using MovieHub.ViewModels;
using Xunit;

namespace MovieHubTests;

public class SurveyControllerTest
{
    private readonly SurveyController _controller;
    private readonly ApplicationDbContext _context;

    public SurveyControllerTest()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("SurveyTestDatabase").Options;
        _context = new ApplicationDbContext(options);   
        _controller = new SurveyController(_context);
    }
    
    // Tests if index-page is loaded
    [Fact]
    public void Test_Index()
    {
        var result = _controller.Index();
        
        // Checks if it returns Index page
        Assert.IsAssignableFrom<IActionResult>(result);
    }
    
    
    [Fact]
    public async void Test_Index_GetAllSurveys()
    {
        // delete database for a fresh start
        await _context.Database.EnsureDeletedAsync();
        
        // Creates 2 new Surveys
        var survey = GetSurvey();
        var survey2 = GetSurvey2();
        
        // added Surveys to Database
        await _controller.Create(survey);
        await _controller.Create(survey2);
        
        // returns List with all surveys from database
        var surveys = await GetSurveysTestAsync();
        
        // Tests if the list contains 2 Surveys
        surveys.Count.Should().Be(2);
        Assert.IsType<Survey>(surveys[0]);
        Assert.IsType<Survey>(surveys[1]);
    }
    
    [Fact]
    public async Task Test_Create()
    {
        // delete database for a fresh start
        await _context.Database.EnsureDeletedAsync();
        
        // Creates a new survey and adds to the database
        var survey = GetSurvey();
        await _controller.Create(survey);
        
        // Gets single survey from Database
        var createdSurvey = await _controller.GetSurvey(survey.Id);
        
        // Checks if the survey is same as the added survey
        createdSurvey.CinemaNumber.Should().Be(survey.CinemaNumber);
        createdSurvey.Email.Should().Be(survey.Email);
        createdSurvey.Id.Should().Be(survey.Id);
        createdSurvey.Hygiene.Should().Be(survey.Hygiene);
        
        await _context.Database.EnsureDeletedAsync();
    }
    
    
    // TEST Functions
    [HttpGet]
    private async Task<List<Survey>> GetSurveysTestAsync()
    {
        return await _context.Survey.ToListAsync();
    }

    [HttpGet]
    private static Survey GetSurvey()
    {
        return new Survey
        {
            Id = 1, CinemaNumber = "CINEMA 1", Email = "test@test.nl", Hygiene = 5, Name = "Test Rob",
            Nuisance = 5, PopcornQuality = 4, ScreenQuality = 4, SoundQuality = 4, TicketPrice = 3,
            TimeStamp = DateTime.Now, ToiletHeight = 2
        };
    }
    
    [HttpGet]
    private static Survey GetSurvey2()
    {
        return new Survey
        {
            Id = 2, CinemaNumber = "CINEMA 2", Email = "test2@test.nl", Hygiene = 2, Name = "Test Rob2",
            Nuisance = 4, PopcornQuality = 2, ScreenQuality = 1, SoundQuality = 5, TicketPrice = 3,
            TimeStamp = DateTime.Now.AddDays(-3), ToiletHeight = 2
        };
    }
    
}
