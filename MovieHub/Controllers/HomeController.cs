using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;
using MovieHub.Data;
using MovieHub.Models;

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

    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.Showtime.Where(s => 
            s.StartAt.Date.Equals(DateTime.Today)).Where(s => 
                s.StartAt.ToLocalTime() > DateTime.Now).Include(s => s.Hall)
            .Include(s => s.Movie).OrderBy(s => s.StartAt);
        return View(await applicationDbContext.ToListAsync());
    }
    public async Task<IActionResult> SearchForMovie(string searchPhrase)
    {
        var applicationDbContext = _context.Showtime.Where(s => 
                s.StartAt.Date.Equals(DateTime.Today)).Where(s => 
                s.StartAt.ToLocalTime() > DateTime.Now).Include(s => s.Hall)
            .Include(s => s.Movie).Where(s=>s.Movie.Title.Contains(searchPhrase)).OrderBy(s => s.StartAt);

        return View("Index", applicationDbContext.ToList());
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