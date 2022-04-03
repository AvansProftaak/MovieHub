#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;
using MovieHub.Models;
using MovieHub.ViewModels;

namespace MovieHub.Controllers
{
    [Authorize(Roles = "Admin, Manager, Back-Office")]
    public class MovieRuntimesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieRuntimesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MovieRuntimes
        public async Task<IActionResult> Index()
        {

            var movieRuntimeViewModel = new MovieRuntimeViewModel
            {
                RuntimeList = _context.MovieRuntime
                    .Include(m => m.Hall)
                    .Include(m => m.Movie)
                    .OrderBy(m => m.MovieId)
                    .ThenBy(m => m.Time)
                    .ToList()
                    
            };

            return View(movieRuntimeViewModel);
        }

        // GET: MovieRuntimes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieRuntime = await _context.MovieRuntime
                .Include(m => m.Hall)
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieRuntime == null)
            {
                return NotFound();
            }

            return View(movieRuntime);
        }

        // GET: MovieRuntimes/Create
        public IActionResult Create()
        {
            ViewData["HallId"] = new SelectList(_context.Hall, "Id", "Id");
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Id");
            return View();
        }

        // POST: MovieRuntimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MovieId,HallId,StartAt,EndAt,Time")] MovieRuntime movieRuntime)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movieRuntime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HallId"] = new SelectList(_context.Hall, "Id", "Id", movieRuntime.HallId);
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Id", movieRuntime.MovieId);
            return View(movieRuntime);
        }

        // GET: MovieRuntimes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieRuntime = await _context.MovieRuntime.FindAsync(id);
            if (movieRuntime == null)
            {
                return NotFound();
            }
            ViewData["HallId"] = new SelectList(_context.Hall, "Id", "Id", movieRuntime.HallId);
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Id", movieRuntime.MovieId);
            return View(movieRuntime);
        }

        // POST: MovieRuntimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovieId,HallId,StartAt,EndAt,Time")] MovieRuntime movieRuntime)
        {
            if (id != movieRuntime.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieRuntime);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieRuntimeExists(movieRuntime.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["HallId"] = new SelectList(_context.Hall, "Id", "Id", movieRuntime.HallId);
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Id", movieRuntime.MovieId);
            return View(movieRuntime);
        }

        // GET: MovieRuntimes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieRuntime = await _context.MovieRuntime
                .Include(m => m.Hall)
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieRuntime == null)
            {
                return NotFound();
            }

            return View(movieRuntime);
        }

        // POST: MovieRuntimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movieRuntime = await _context.MovieRuntime.FindAsync(id);
            _context.MovieRuntime.Remove(movieRuntime);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieRuntimeExists(int id)
        {
            return _context.MovieRuntime.Any(e => e.Id == id);
        }
    }
}
