using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;
using MovieHub.Models;

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

//
// public IActionResult Create()
//     {
//         return View();
//     }
//
//     [HttpPost]
//     [ValidateAntiForgeryToken]
//     public async Task<IActionResult> Create([Bind("Id, Description, Icon")] Pegi pegi)
//     {
//         if (ModelState.IsValid)
//         {
//             _context.Add(pegi);
//             await _context.SaveChangesAsync();
//             return RedirectToAction(nameof(Index));
//         }
//
//         return View(pegi);
//     }
}