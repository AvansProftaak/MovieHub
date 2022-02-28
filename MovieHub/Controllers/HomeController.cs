using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieHub.Data;
using MovieHub.Models;
using MovieHub.ViewModel;

namespace MovieHub.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    public IndexViewModel IndexViewModel { get; set; }

    public HomeController(ILogger<HomeController> logger,
        ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<ActionResult<IndexViewModel>> Index()
    {
        var movieDao = new MovieDao();

// Get now playing movies
        ViewBag.NowCinema1 = MovieDao.MovieNow(1).Title;
        ViewBag.NowCinema2 = MovieDao.MovieNow(2).Title;
        ViewBag.NowCinema3 = MovieDao.MovieNow(3).Title;
        ViewBag.NowCinema4 = MovieDao.MovieNow(4).Title;
        ViewBag.NowCinema5 = MovieDao.MovieNow(5).Title;
        ViewBag.NowCinema6 = MovieDao.MovieNow(6).Title;

// Get next playing movies
        ViewBag.NextCinema1 = MovieDao.MovieNext(1).Title;
        ViewBag.NextCinema2 = MovieDao.MovieNext(2).Title;
        ViewBag.NextCinema3 = MovieDao.MovieNext(3).Title;
        ViewBag.NextCinema4 = MovieDao.MovieNext(4).Title;
        ViewBag.NextCinema5 = MovieDao.MovieNext(5).Title;
        ViewBag.NextCinema6 = MovieDao.MovieNext(6).Title;

        if (ViewBag.NextCinema1 == null) { ViewBag.NextCinema1 = "-"; } 
        if (ViewBag.NextCinema2 == null) { ViewBag.NextCinema2 = "-"; } 
        if (ViewBag.NextCinema3 == null) { ViewBag.NextCinema3 = "-"; } 
        if (ViewBag.NextCinema4 == null) { ViewBag.NextCinema4 = "-"; } 
        if (ViewBag.NextCinema5 == null) { ViewBag.NextCinema4 = "-"; } 
        if (ViewBag.NextCinema6 == null) { ViewBag.NextCinema6 = "-"; }
        if (ViewBag.NowCinema1 == null) { ViewBag.NowCinema1 = "-"; } 
        if (ViewBag.NowCinema2 == null) { ViewBag.NowCinema2 = "-"; } 
        if (ViewBag.NowCinema3 == null) { ViewBag.NowCinema3 = "-"; } 
        if (ViewBag.NowCinema4 == null) { ViewBag.NowCinema4 = "-"; } 
        if (ViewBag.NowCinema5 == null) { ViewBag.NowCinema5 = "-"; } 
        if (ViewBag.NowCinema6 == null) { ViewBag.NowCinema6= "-"; }
        
        
        IndexViewModel IndexViewModel = new();

        IndexViewModel.showtime = _context.Showtime
            .Where(s => s.StartAt.Date.Equals(DateTime.Today))
            .Where(s => s.StartAt.ToLocalTime() > DateTime.Now)
            .Include(s => s.Hall)
            .Include(s => s.Movie)
            .OrderBy(s => s.StartAt);
        
        // IndexViewModel.rob = 
        

        return View(IndexViewModel);
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
}