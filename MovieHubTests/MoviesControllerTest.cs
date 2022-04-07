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
    private readonly ApplicationDbContext _context;

    public MoviesControllerTest()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("MoviesTestDatabase")
            .EnableSensitiveDataLogging()
            .Options;
        _context = new ApplicationDbContext(options);
        _controller = new MoviesController(_context);
    }
    
    // Tests if index-page is loaded and shows all Movies 
    [Fact]
    public async void Test_Index()
    {
        var result = _controller.Index();
        await Assert.IsType<Task<IActionResult>>(result);

    }
    
    // Test the Details function
    [Fact]
    public async void Test_Details()
    {
        // Delete the database for a fresh start
        await _context.Database.EnsureDeletedAsync();
        
        var movie = SetMovie();
        var result = _controller.Details(movie.Id);
        await Assert.IsType<Task<IActionResult>>(result);
        await _context.Database.EnsureDeletedAsync();
    }


    [Fact]
    public async void Test_Create()
    {
        // Delete the database for a fresh start
        await _context.Database.EnsureDeletedAsync();

        // Fill pegi and genres tables (2 of both)
        await SetPegis();
        await SetGenres();

        var movie = SetMovie();
        
        var pegis = PegiIdList();
        var genres = GenreIdList();

        // This creates a new Movie, including all Genres and Pegis 
        await _controller.Create(movie, pegis, genres);

        // Gets a list of Movies, Pegis and Genres
        var movieList = await _controller.GetMoviesAsync();
        var moviePegiList = await GetMoviePegis();
        var movieGenreList = await GetMovieGenres();

        // Test passes if movieList has 1 item, moviePegiList 2 and movieGenreList 2
        movieList.Count.Should().Be(1);
        moviePegiList.Count.Should().Be(2);
        movieGenreList.Count.Should().Be(2);

    }
    

    [Fact]
    public async void Test_Delete()
    {
        //empty database
        await _context.Database.EnsureDeletedAsync();
        
        //Add 2 movies to the database
        await SetPegis();
        await SetGenres();
        
        await _controller.Create(SetMovie(), PegiIdList(), GenreIdList());
        await _controller.Create(SetMovie2(), PegiIdList2(), GenreIdList2());
        
        //Test if there are 2 movies in database
        var movies = await _controller.GetMoviesAsync();
        movies.Count.Should().Be(2);
        
        //Test if there are 4 moviePegis and 4 movieGenres in database (2 per movie)
        var moviePegis = GetMoviePegis();
        var movieGenres = GetMovieGenres();
        moviePegis.Result.Count.Should().Be(4);
        movieGenres.Result.Count.Should().Be(4);
        
        // Delete Movie with id=1 and return the movieList again
        await _controller.DeleteConfirmed(1);
        movies = await _controller.GetMoviesAsync();
        
        // Now there must be one movie in the list and it has id=2
        movies.Count.Should().Be(1);
        movies.First().Id.Should().Be(2);

        // And also check if 2 moviePegis and 2 movieGenres were deleted (we have 2 left of both)
        moviePegis = GetMoviePegis();
        movieGenres = GetMovieGenres();
        moviePegis.Result.Count.Should().Be(2);
        movieGenres.Result.Count.Should().Be(2);
    }
    
    
    
    //TEST Functions
    [HttpGet]
    private async Task<List<MoviePegi>> GetMoviePegis()
    {
        return await _context.MoviePegi.ToListAsync();
    }
    
    [HttpGet]
    private async Task<List<MovieGenre>> GetMovieGenres()
    {
        return await _context.MovieGenre.ToListAsync();
    }
    
    [HttpPost]
    private async Task SetPegis()
    {
        await _context.AddAsync(Pegis()[0]);
        await _context.AddAsync(Pegis()[1]);
        await _context.SaveChangesAsync();
    }
    
    [HttpPost]
    private async Task SetGenres()
    {
        await _context.AddAsync(Genres()[0]);
        await _context.AddAsync(Genres()[1]);
        await _context.SaveChangesAsync();
    }



    //OBJECTS:
    private static Movie SetMovie()
    {
        return new Movie
        {
            Id = 1,
            Title = "Blacklight",
            Description = "Blacklight Description",
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
    
    private static Movie SetMovie2()
    {
        return new Movie
        {
            Id = 2,
            Title = "Encanto (NL)",
            Description = "Encanto (NL) Description",
            Duration = 100,
            Cast = "Stephanie Beatriz",
            Director = "Jared Bush",
            ImdbScore = 7.3,
            ReleaseDate = DateTime.Now.AddDays(-2),
            Is3D = true,
            IsSecret = false,
            Language = "Dutch",
            ImageUrl = "https://i.ibb.co/TPRKy0G/encanto.jpg",
            TrailerUrl = "https://www.youtube.com/watch?v=CaimKeDcudo"
        };
    }

    private static int[] PegiIdList()
    {
        return new[]
        {
            1,
            2
        };
    }

    private static int[] GenreIdList()
    {
        return new[]
        {
            1,
            2
        };
    }
    
    private static int[] PegiIdList2()
    {
        return new[]
        {
            3,
            4
        };
    }

    private static int[] GenreIdList2()
    {
        return new[]
        {
            3,
            4
        };
    }
    

    private static List<Pegi> Pegis()
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
    
    private static List<Genre> Genres()
    {
        return new List<Genre>
        {
            new()
            {
                Id = 1,
                Name = "Action"
            },
            new()
            {
                Id = 2,
                Name = "Adventure"
            }
        };
    }
}