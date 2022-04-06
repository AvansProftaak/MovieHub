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

public class MoviesControllerTest
{
    private readonly MoviesController _controller;

    public MoviesControllerTest()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MoviesDatabase")
            .Options;
        var context = new ApplicationDbContext(options);
        _controller = new MoviesController(context);
    }

    [Fact]
    public void Details_Returns_Details_View()
    {
        var movie = GetMovie();
        var result = _controller.Details(movie.Id);
        Assert.IsType<Task<IActionResult>>(result);
    }
    
    [Fact]
    public async void Test_Create_And_Delete_Movie()
    {
        var movie = GetMovie();
        await _controller.CreateMovieAsync(movie);
        
        //Test if Title, Director and Id from List equals object 
        var movies = await _controller.GetMoviesAsync();
        movies.First().Title.Should().Be(movie.Title);
        movies.First().Director.Should().Be(movie.Director);
        movies.First().Id.Should().Be(movie.Id);

        //Get created Movie and compare with input
        var createdMovie = await _controller.GetMovieAsync(movie.Id);
        createdMovie.Title.Should().Be(movie.Title);
        createdMovie.Director.Should().Be(movie.Director);
        createdMovie.Id.Should().Be(movie.Id);
        
        // Now there must be one movie in the list
        movies.Count.Should().Be(1);
        
        // Delete the movie and return the movieList again
        await _controller.DeleteConfirmed(movie.Id);
        movies = await _controller.GetMoviesAsync();
        
        // Now there must be one movie in the list
        movies.Count.Should().Be(0);
        
        //If all above Controller-functions work, test passes
    }
    
    
    
    //OBJECTS:
    private static Movie GetMovie()
    {
        return new Movie
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
        };
    }
    
    private static MovieRuntime GetMovieRuntime()
    {
        return new MovieRuntime
        {
            Id = 1,
            MovieId = 1,
            HallId = 1,
            StartAt = DateTime.Today.Date,
            EndAt = DateTime.Today.AddDays(1).Date,
            Time = TimeSpan.FromHours(23)
        };
    }

    private static List<Showtime> GetShowTimes()
    {
        return new List<Showtime>
        {
            new()
            {
                HallId = 1,
                MovieId = 1,
                StartAt = DateTime.Today.AddHours(23)
            },
            new()
            {
                HallId = 1,
                MovieId = 1,
                StartAt = DateTime.Today.AddHours(23)
            }
        };
    }

    private static List<Pegi> GetPegis()
    {
        return new List<Pegi>
        {
            new()
            {
                Id = 1,
                Description = "Gambling",
                Icon = "https://static.wikia.nocookie.net/rating-system/images/4/40/PEGI6.jpg"
            },
            new()
            {
                Id = 2,
                Description = "Violence",
                Icon = "https://static.wikia.nocookie.net/rating-system/images/e/e8/PEGI3.jpg"
            }
        };
    }

    private static List<MoviePegi> GetMoviePegis()
    {
        return new List<MoviePegi>
        {
            new()
            {
                MovieId = 1,
                PegiId = 1
            },
            new()
            {
                MovieId = 1,
                PegiId = 2
            }
        };
    }

    private static List<Genre> GetGenres()
    {
        return new List<Genre>
        {
            new()
            {
                Name = "Action"
            },
            new()
            {
                Name = "Adventure"
            }
        };
    }

    private static List<MovieGenre> GetMovieGenres()
    {
        return new List<MovieGenre>
        {
            new()
            {
                MovieId = 1,
                GenreId = 1
            },
            new()
            {
                MovieId = 1,
                GenreId = 2            
            }
        };
    }
}