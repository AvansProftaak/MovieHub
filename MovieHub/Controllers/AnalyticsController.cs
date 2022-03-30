using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;
using MovieHub.ViewModels;

namespace MovieHub.Controllers;

[Authorize]
public class AnalyticsController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public AnalyticsController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult HallAnalytics()
    {
        var vm = new HallAnalyticsViewModel
        {
            Halls = _context.Hall.ToList().OrderBy(h => h.Id)
        };
        return View(vm);
    }
}