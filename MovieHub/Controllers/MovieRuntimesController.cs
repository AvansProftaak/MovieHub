#nullable disable

using Humanizer;
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
        public IActionResult Index()
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
            ViewData["HallId"] = new SelectList(_context.Hall.OrderBy(h => h.Id), "Id", "Name");
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

            var runtimeList = _context.MovieRuntime.Where(m => m.HallId == movieRuntime.HallId).Include(m => m.Movie).ToList();
            var movie = _context.Movie.FirstOrDefault(m => m.Id == movieRuntime.MovieId);
            var hall = _context.Hall.FirstOrDefault(h => h.Id == movieRuntime.HallId);

            foreach (var runtime in runtimeList)
            {
                if (
                    // if startDate new Runtime between startDate AND endDate of other Runtimes
                    (movieRuntime.StartAt >= runtime.StartAt && movieRuntime.StartAt <= runtime.EndAt || 
                    // OR if endDate new Runtime between startDate AND endDate of other Runtimes
                     movieRuntime.EndAt >= runtime.StartAt && movieRuntime.EndAt <= runtime.EndAt) && 
                    // AND startTime new Runtime is between startTime AND endTime of other Runtimes
                    (time >= runtime.Time && time <= runtime.Time.Add(runtime.Movie.Duration.Minutes()) || 
                    // OR endTime new Runtime is between startTime AND endTime of other Runtime
                     time.Add(movie.Duration.Minutes()) >= runtime.Time && time.Add(movie.Duration.Minutes()) <= runtime.Time.Add(runtime.Movie.Duration.Minutes())))
                    
                {
                    // Show errormessage in View
                    TempData["ErrorMessage"] = @hall.Name + " is not available on this time";
                    
                    // Fills dropdown buttons
                    ViewData["HallId"] = new SelectList(_context.Hall.OrderBy(h => h.Id), "Id", "Name");
                    ViewData["MovieId"] = new SelectList(_context.Movie.OrderBy(m => m.Id), "Id", "Title");
                    
                    return RedirectToAction(nameof(Create));
                }
            }
            
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
                    StartAt = date.Add(time).ToUniversalTime(),
                    MovieRuntimeId = newMovieRuntime.Id
                };
                _context.Showtime?.Add(showtime);
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
            ViewData["HallId"] = new SelectList(_context.Hall, "Id", "Name", movieRuntime.HallId);
            ViewData["MovieId"] = new SelectList(_context.Movie, "Id", "Title", movieRuntime.MovieId);
            return View(movieRuntime);
        }

        // POST: MovieRuntimes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovieId,HallId,StartAt,EndAt")] MovieRuntime movieRuntime, TimeSpan time)
        {
            if (id != movieRuntime.Id)
            {
                return NotFound();
            }
            
            var runtimeList = _context.MovieRuntime
                .Where(m => m.HallId == movieRuntime.HallId)
                .Include(m => m.Movie)
                .ToList();
            var showtimeList = _context.Showtime
                .Where(s => s.MovieRuntimeId == movieRuntime.Id)
                .Where(s => s.StartAt.ToLocalTime() >= DateTime.Now)
                .ToList();
            var movie = _context.Movie
                .FirstOrDefault(m => m.Id == movieRuntime.MovieId);
            var hall = _context.Hall
                .FirstOrDefault(h => h.Id == movieRuntime.HallId);

            foreach (var runtime in runtimeList)
            {
                if ( runtime.Id != id &&
                    // if startDate new Runtime between startDate AND endDate of other Runtimes
                    (movieRuntime.StartAt >= runtime.StartAt && movieRuntime.StartAt <= runtime.EndAt || 
                     // OR if endDate new Runtime between startDate AND endDate of other Runtimes
                     movieRuntime.EndAt >= runtime.StartAt && movieRuntime.EndAt <= runtime.EndAt) && 
                    // AND startTime new Runtime is between startTime AND endTime of other Runtimes
                    (time >= runtime.Time && time <= runtime.Time.Add(runtime.Movie.Duration.Minutes()) || 
                     // OR endTime new Runtime is between startTime AND endTime of other Runtime
                     time.Add(movie.Duration.Minutes()) >= runtime.Time && time.Add(movie.Duration.Minutes()) <= runtime.Time.Add(runtime.Movie.Duration.Minutes())))                    
                {
                    // Show errormessage in View
                    TempData["ErrorMessage"] = @hall.Name + " is not available on this time";
                    
                    // Fills dropdown buttons
                    ViewData["HallId"] = new SelectList(_context.Hall.OrderBy(h => h.Id), "Id", "Name");
                    ViewData["MovieId"] = new SelectList(_context.Movie.OrderBy(m => m.Id), "Id", "Title");
                    
                    return RedirectToAction(nameof(Edit));
                }
            }
            
            var oldMovieRuntime = _context.MovieRuntime
                .FirstOrDefault(m => m.Id == id);
            _context.Remove(oldMovieRuntime!);
            
            foreach (var showtime in showtimeList)
            {
                _context.Showtime.Remove(showtime);
            }
            
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
                    StartAt = date.Add(time).ToUniversalTime(),
                    MovieRuntimeId = newMovieRuntime.Id
                };
                _context.Showtime?.Add(showtime);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
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
            _context.MovieRuntime.Remove(movieRuntime!);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieRuntimeExists(int id)
        {
            return _context.MovieRuntime.Any(e => e.Id == id);
        }
    }
}
