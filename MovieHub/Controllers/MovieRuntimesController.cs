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
                Halls = _context.Hall.OrderBy(h => h.Id).ToList(),
                RuntimeList = _context.MovieRuntime
                    .Include(m => m.Hall)
                    .Include(m => m.Movie)
                    .OrderBy(m => m.Time)
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
            ViewData["HallId"] = new SelectList(_context.Hall.OrderBy(h => h.Id), "Id", "Id");
            ViewData["MovieId"] = new SelectList(_context.Movie.OrderBy(m => m.Id), "Id", "Title");
            return View();
        }

        // POST: MovieRuntimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId,HallId,StartAt,EndAt")] MovieRuntime movieRuntime, TimeSpan time)
        {
            
            var newMovieRuntime = new MovieRuntime
            {
                MovieId = movieRuntime.MovieId,
                HallId = movieRuntime.HallId,
                StartAt = movieRuntime.StartAt,
                EndAt = movieRuntime.EndAt,
                Time = time
            };
            _context.Add(newMovieRuntime);
            await _context.SaveChangesAsync();

            var dates = new List<DateTime>();
            
            for (var dt = movieRuntime.StartAt; dt <= movieRuntime.EndAt; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }
            
            foreach (var date in dates)
            {
                var showtime = new Showtime
                {
                    HallId = movieRuntime.HallId,
                    MovieId = movieRuntime.MovieId,
                    StartAt = date.Add(time).ToUniversalTime()
                };
                Console.WriteLine(showtime.StartAt);
                
                _context.Showtime.Add(showtime);
                await _context.SaveChangesAsync();
            }
            

            return RedirectToAction(nameof(Index));
                
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
