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
        
        public async Task<IActionResult> Index()
        {

            // Gives back a list with all Halls and all MovieRuntimes
            var movieRuntimeViewModel = new MovieRuntimeViewModel
            {
                Halls = _context.Hall.OrderBy(h => h.Id).ToList(),
                MovieRuntimes = await _context.MovieRuntime
                    .Include(m => m.Hall)
                    .Include(m => m.Movie)
                    .OrderBy(m => m.Time)
                    .ToListAsync()
            };

            return View(movieRuntimeViewModel);
        }

        
        public async Task<IActionResult> Details(int id)
        {
            return View(await GetOneMovieRuntimeAsync(id));
        }
        
        public IActionResult Create()
        {
            ViewData["HallId"] = new SelectList(_context.Hall.OrderBy(h => h.Id), "Id", "Name");
            ViewData["MovieId"] = new SelectList(_context.Movie.OrderBy(m => m.Id), "Id", "Title");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId,HallId,StartAt,EndAt")] MovieRuntime movieRuntime, TimeSpan time)
        {
            var hallName = _context.Hall.FirstOrDefault(h => h.Id == movieRuntime.HallId)?.Name;

            // Checks if Runtime is available in schedule
            if (await CheckAvailableRuntime(movieRuntime, time))
            {
                // Show errormessage in View
                TempData["ErrorMessage"] = hallName + " is not available on this time";
                    
                // Fills dropdown buttons
                ViewData["HallId"] = new SelectList(_context.Hall.OrderBy(h => h.Id), "Id", "Name");
                ViewData["MovieId"] = new SelectList(_context.Movie.OrderBy(m => m.Id), "Id", "Title");
                
                // Back to the Create View and show an error
                return RedirectToAction(nameof(Create));
            }
            
            // Creates a MovieRuntime 
            var newMovieRuntime = await CreateMovieRuntimeAsync(movieRuntime, time);
            
            // Creates new Showtimes with the new MovieRuntime
            await CreateShowtimesAsync(newMovieRuntime, time);
            
            // Back to index-page
            return RedirectToAction(nameof(Index));
        }
        
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovieId,HallId,StartAt,EndAt")] MovieRuntime movieRuntime, TimeSpan time)
        {
            var hallName = _context.Hall.FirstOrDefault(h => h.Id == movieRuntime.HallId)?.Name;

            // Checks if edited Runtime is available in schedule
            if (await CheckAvailableRuntime(movieRuntime, time))
            {
                // Show errormessage in View
                TempData["ErrorMessage"] = hallName + " is not available on this time";
                    
                // Fills dropdown buttons
                ViewData["HallId"] = new SelectList(_context.Hall.OrderBy(h => h.Id), "Id", "Name");
                ViewData["MovieId"] = new SelectList(_context.Movie.OrderBy(m => m.Id), "Id", "Title");
                
                // Back to the Create View and show an error
                return RedirectToAction(nameof(Edit));
            }
            
            // Deletes old MovieRuntime and Showtimes before adding new one
            await DeleteConfirmed(id);
            await DeleteShowTimes(id);

            // Creates a MovieRuntime 
            var newMovieRuntime = await CreateMovieRuntimeAsync(movieRuntime, time);
            
            // Creates new Showtimes with the new MovieRuntime
            await CreateShowtimesAsync(newMovieRuntime, time);
         
            // Back to index-page
            return RedirectToAction(nameof(Index));
        }

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

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movieRuntime = await _context.MovieRuntime.FindAsync(id);
            _context.MovieRuntime.Remove(movieRuntime!);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Checks if the given Movie Runtime is available in the movie schedule
        public Task<bool> CheckAvailableRuntime(MovieRuntime movieRuntime, TimeSpan time)
        {
            var runtimeList = _context.MovieRuntime
                .Where(m => m.HallId == movieRuntime.HallId)
                .Include(m => m.Movie)
                .Include(m => m.Hall)
                .ToList();

            return Task.FromResult(runtimeList.Any(runtime => (runtime.Id != movieRuntime.Id || movieRuntime.Id == 0) &&
                                                              // if startDate new Runtime between startDate AND endDate of other Runtimes
                                                              (movieRuntime.StartAt >= runtime.StartAt && movieRuntime.StartAt <= runtime.EndAt ||
                                                               // OR if endDate new Runtime between startDate AND endDate of other Runtimes
                                                               movieRuntime.EndAt >= runtime.StartAt && movieRuntime.EndAt <= runtime.EndAt) &&
                                                              // AND startTime new Runtime is between startTime AND endTime of other Runtimes
                                                              (time >= runtime.Time && time <= runtime.Time.Add(runtime.Movie.Duration.Minutes()) ||
                                                               // OR endTime new Runtime is between startTime AND endTime of other Runtime
                                                               time.Add(runtime.Movie.Duration.Minutes()) >= runtime.Time && time.Add(runtime.Movie.Duration.Minutes()) <= runtime.Time.Add(runtime.Movie.Duration.Minutes()))));
        }
        
        //Saves the MovieRuntime to the Database
        public async Task<MovieRuntime> CreateMovieRuntimeAsync(MovieRuntime movieRuntime, TimeSpan time)
        {
            var result = new MovieRuntime
            {
                MovieId = movieRuntime.MovieId,
                HallId = movieRuntime.HallId,
                StartAt = movieRuntime.StartAt,
                EndAt = movieRuntime.EndAt,
                Time = time
            };            
            await _context.AddAsync(result);
            await _context.SaveChangesAsync();

            return result;
        }
        
        //Creates a showtime for each MovieRuntime in selected period
        public async Task CreateShowtimesAsync(MovieRuntime movieRuntime, TimeSpan time)
        {
            var dates = new List<DateTime>();
                  
            // Foreach date in selected dates, add it to the dateList
            for (var dt = movieRuntime.StartAt; dt <= movieRuntime.EndAt; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }

            // Foreach date from above list, add a Showtime to the Showtime table
            foreach (var date in dates)
            {
                var showtime = new Showtime
                {
                    HallId = movieRuntime.HallId,
                    MovieId = movieRuntime.MovieId,
                    StartAt = date.Add(time).ToUniversalTime(),
                    MovieRuntimeId = movieRuntime.Id
                };
                await _context.Showtime!.AddAsync(showtime);
                await _context.SaveChangesAsync();
            }
        }
        
        //Deletes the showtimes when MovieRuntime is edited or removed
        public Task DeleteShowTimes(int id)
        {
            var showtimeList = _context.Showtime?
                .Where(s => s.MovieRuntimeId == id)
                .Where(s => s.StartAt.ToLocalTime() >= DateTime.Now)
                .ToList();
            
            foreach (var showtime in showtimeList!)
            {
                _context.Showtime?.Remove(showtime);
            }

            return Task.CompletedTask;
        }

        // Gets all 
        public async Task<IList<MovieRuntime>> GetMovieRuntimesAsync()
        {
            return await _context.MovieRuntime
                .OrderBy(m => m.Time)
                .ToListAsync();
        }
        
        public async Task<MovieRuntime> GetOneMovieRuntimeAsync(int id)
        {
            return await _context.MovieRuntime
                .Where(m => m.Id == id)
                .Include(m => m.Hall)
                .Include(m => m.Movie)
                .OrderBy(m => m.Time)
                .FirstAsync();
        }
    }
}
