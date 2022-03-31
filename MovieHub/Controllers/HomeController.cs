using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MovieHub.Data;
using MovieHub.Models;
using MovieHub.ViewModels;

using Npgsql;

namespace MovieHub.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger,
        ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<ActionResult<IndexViewModel>> Index()
    {
        IndexViewModel indexViewModel = new IndexViewModel();
        
        indexViewModel.MovieIndex = MovieIndex();
        // indexViewModel.SearchForMovie = SearchForMovie (searchPhrase);
        indexViewModel.Halls = GetHalls();
        indexViewModel.Movies = GetMovies();
        indexViewModel.ShowNext = ShowNext();
        indexViewModel.ShowNow = ShowNow();
        indexViewModel.MovieRuntimes = GetAllMovieRuntimes();

        return View(indexViewModel);
        }

    public List<Movie> MovieIndex()
    {

        DateTime date = DateTime.Today;
        var firstday = GetFirstDayOfWeek(date);
        var lastday = GetLastDayOfWeek(date);

        // return _context.Showtime
        var showtime = _context.Showtime

            .Where(s => (s.StartAt.ToLocalTime() >= firstday))
            .Where(s => s.StartAt.ToLocalTime() <= lastday)
            .Include(s => s.Hall)
            .Include(s => s.Movie)
            .OrderBy(s => s.StartAt);

        var ThisWeeksMovieList = new List<Movie>();

        foreach (var item in showtime)
        {
            ThisWeeksMovieList.Add(item.Movie);
        }

        var thisWeeksMovieListDistinct = ThisWeeksMovieList.Distinct();
            return  thisWeeksMovieListDistinct.ToList();
        
    }
    
    
    // public IOrderedQueryable<Showtime> SearchForMovie(string searchPhrase)
    // {
    // DateTime date = DateTime.Today;
    // var firstday = GetFirstDayOfWeek(date);
    // var lastday = GetLastDayOfWeek(date);
    //     
    //     return _context.Showtime
    //         .Where(s => (s.StartAt.ToLocalTime() >= firstday))
    //         .Where(s => s.StartAt.ToLocalTime() <= lastday)
    //          .Include(s => s.Hall)
    //          .Include(s => s.Movie)
    //          .OrderBy(s => s.StartAt);
    //          .Where(s=>s.Movie.Title.Contains(searchPhrase))
    //          .OrderBy(s => s.StartAt);
    // } 
    public List<Hall> GetHalls()
    {
        return _context.Hall.OrderBy(h => h.Id).ToList();
    }
    
    public List<Movie> GetMovies()
    {
        return _context.Movie.OrderBy(m => m.Id).ToList();
    }
    
    public List<Showtime> ShowNext()
    {
        return _context.Showtime!
                 .FromSqlRaw("SELECT x.* FROM (SELECT *, ROW_NUMBER() OVER (PARTITION BY \"HallId\" ORDER BY \"StartAt\") rn FROM public.\"Showtime\" where \"StartAt\" > now()) x JOIN public.\"Movie\" M ON \"MovieId\" = M.\"Id\" WHERE x.rn = 1 ORDER BY \"HallId\"").ToList();
    }

    public List<Showtime> ShowNow()
    {
        return _context.Showtime!
            .FromSqlRaw(
                "SELECT x.* FROM (SELECT *, ROW_NUMBER() OVER (PARTITION BY \"HallId\" ORDER BY \"StartAt\" DESC ) rn FROM public.\"Showtime\" WHERE \"StartAt\" < now()) x JOIN public.\"Movie\" M ON \"MovieId\" = M.\"Id\" WHERE x.rn = 1 ORDER BY \"HallId\"").ToList();
    }
    
    
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
    
    public static DateTime GetFirstDayOfWeek(DateTime date)
    {
        var firstday = DayOfWeek.Monday;

        var diff = date.DayOfWeek - firstday;
        if (diff < 0)
            diff += 7;
        return date.AddDays(-diff).Date;
    }

    //To Get The Last Day of the Week in C#
    public static DateTime GetLastDayOfWeek(DateTime date)
    {
        var firstday = DayOfWeek.Monday;
        var diff = date.DayOfWeek - firstday;

        if (diff < 0)
            diff += 7;
        DateTime start = date.AddDays(-diff).Date;
        
        // Add 6 days to get the last day, but to display all movies from the last day we add 7!
        return start.AddDays(7).Date;
    }

    public List<MovieRuntime?> GetAllMovieRuntimes()
    {
        return _context.MovieRuntime.ToList()!;
    }

    public async Task<IActionResult> InsertEmail(string email)
    {
        var result = await _context.Newsletter.FirstOrDefaultAsync(p => p.Email == email);
        if(result != null)
        {
            return BadRequest();
        } 
        else
        {
            Newsletter newsletter = new Newsletter
            {
                Email = email,
            };
            await _context.Newsletter.AddAsync(newsletter);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}