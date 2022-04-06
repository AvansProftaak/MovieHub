using System;
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
    
    [Fact]
    public async Task Test_Should_Add_Survey_To_Db()
    {
        // Creates a new survey
        var survey = GetSurvey();
        
        // added Survey to Database
        await _controller.CreateSurveyAsync(survey);
        
        // returns List from Database
        var surveys = await _controller.GetSurveysAsync();
        
        // Tests if the first survey from List is same as the added survey
        surveys.First().CinemaNumber.Should().Be(survey.CinemaNumber);
        surveys.First().Email.Should().Be(survey.Email);
        surveys.First().Id.Should().Be(survey.Id);
        surveys.First().Hygiene.Should().Be(survey.Hygiene);
        
        // Gets single survey from Database
        var createdSurvey = await _controller.GetSurveyAsync(survey.Id);
        
        // Checks if the survey is same as the added survey
        createdSurvey.CinemaNumber.Should().Be(survey.CinemaNumber);
        createdSurvey.Email.Should().Be(survey.Email);
        createdSurvey.Id.Should().Be(survey.Id);
        createdSurvey.Hygiene.Should().Be(survey.Hygiene);
        
        await _context.Database.EnsureDeletedAsync();
    }
    
    [Fact]
    public async void Test_Should_Return_Details_View()
    {
        // Creates a new survey
        var survey = GetSurvey();
        
        // added Survey to Database
        await _controller.CreateSurveyAsync(survey);
        
        var result = _controller.Details(survey.Id);
        await Assert.IsType<Task<IActionResult>>(result);
        await _context.Database.EnsureDeletedAsync();
    }
    
    private static Survey GetSurvey()
    {
        return new Survey
        {
            Id = Guid.NewGuid().GetHashCode(), CinemaNumber = "CINEMA 1", Email = "test@test.nl", Hygiene = 5, Name = "Test Rob",
            Nuisance = 5, PopcornQuality = 4, ScreenQuality = 4, SoundQuality = 4, TicketPrice = 3,
            TimeStamp = DateTime.Now, ToiletHeight = 2
        };
    }
}