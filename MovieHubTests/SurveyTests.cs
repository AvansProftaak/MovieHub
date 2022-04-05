using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MovieHub;
using MovieHub.Data;
using MovieHub.Models;
using Xunit;

namespace MovieHubTests;

public class SurveyTests
{
    private readonly SurveyController _controller;

    public SurveyTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase").Options;
        var context = new ApplicationDbContext(options);   
        _controller = new SurveyController(context);
    }
    
    [Fact]
    public async Task CreateShouldAddSurveyToDb()
    {
        var survey = GetSurvey();
        await _controller.CreateSurveyAsync(survey);

        var surveys = await _controller.GetSurveysAsync();

        surveys.Single().CinemaNumber.Should().Be(survey.CinemaNumber);
        surveys.Single().Email.Should().Be(survey.Email);
        surveys.Single().Id.Should().Be(survey.Id);
        surveys.Single().Hygiene.Should().Be(survey.Hygiene);

        var createdSurvey = await _controller.GetSurveyAsync(survey.Id);
        
        createdSurvey.CinemaNumber.Should().Be(survey.CinemaNumber);
        createdSurvey.Email.Should().Be(survey.Email);
        createdSurvey.Id.Should().Be(survey.Id);
        createdSurvey.Hygiene.Should().Be(survey.Hygiene);
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