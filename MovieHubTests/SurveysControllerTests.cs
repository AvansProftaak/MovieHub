using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieHub.Controllers;
using MovieHub.Data;
using MovieHub.Models;
using Xunit;

namespace MovieHubTests;

public class SurveysControllerTests
{
    private readonly SurveysController _controller;
    private readonly ApplicationDbContext _context;
    
    public SurveysControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("SurveyTestDatabase").Options;
        _context = new ApplicationDbContext(options);
        _controller = new SurveysController(_context);
    }
    
    [Fact]
    public void Test_Surveys_Database_Ok()
    {
        _context.Database.EnsureCreated();
        InsertTestData(_context);
        Assert.Equal("Peter de Vries", _context.Survey.First().Name);
    }

    [Fact]
    public async Task Test_Survey_Controller()
    {
        // Ensure database is empty on start of test
        await _context.Database.EnsureDeletedAsync();

        // generate Survey and create it
        var survey = NewSurvey();
        await _controller.Create(survey);

        // retrieve all survey responses
        var surveyResponses = _controller.GetSurveyResponses().Result;

        // Verify if create and get functions actually created and retrieved the survey record
        Assert.Contains(surveyResponses, item => item.Email == "merelvolkers@gmail.com");
        
        // Ensure database is empty on end of test
        await _context.Database.EnsureDeletedAsync();
    }

    private void InsertTestData(ApplicationDbContext context)
    {
        context.Add(new Survey()
        {
            Id = 1,
            Age = 30,
            Gender = GenderEnum.Male,
            Name = "Peter de Vries",
            Email = "peterdevries@gmail.com",
            Facilities = 4,
            Hygiene = 5,
            FoodDrinks = 3,
            Staff = 4,
            ScreenQuality = 5,
            SoundQuality = 5,
            Price = 2,
            Remark = "I think the 3D supplement is ridiculously expensive!",
            SurveyFilledIn = DateTime.UtcNow
        });
        
        context.Add(new Survey()
        {
            Id = 2,
            Age = 57,
            Gender = GenderEnum.Female,
            Name = "Sandra Peters",
            Email = "sandrapeters@gmail.com",
            Facilities = 2,
            Hygiene = 1,
            FoodDrinks = 5,
            Staff = 5,
            ScreenQuality = 4,
            SoundQuality = 4,
            Price = 4,
            Remark = "There was no more toilet paper in the bathrooms.",
            SurveyFilledIn = DateTime.UtcNow
        });
        
        context.SaveChanges();
    }

    private Survey NewSurvey()
    {
        return new Survey
        {
            Id = 1,
            Age = 40,
            Gender = GenderEnum.Female,
            Name = "Merel Volkers",
            Email = "merelvolkers@gmail.com",
            Facilities = 4,
            Hygiene = 5,
            FoodDrinks = 5,
            Staff = 5,
            ScreenQuality = 4,
            SoundQuality = 4,
            Price = 3,
            Remark = "The guy selling popcorn was really friendly and helpful."
        };
    }
}