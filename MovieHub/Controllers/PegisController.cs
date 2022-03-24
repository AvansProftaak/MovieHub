using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;

namespace MovieHub.Controllers;

public class PegisController : Controller
{
    private readonly ApplicationDbContext _context;

    public PegisController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Pegis
    public async Task<IActionResult> Index()
    {
        return View(await _context.Pegi.ToListAsync());
    }
}