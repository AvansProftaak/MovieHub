using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieHub.Data;
using MovieHub.Models;
using MovieHub.ViewModels;


namespace MovieHub.Controllers
{
    [Authorize(Roles = "Admin, Manager, Back-Office")]
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: Movies
        public async Task<IActionResult> Index()
        {
           return View(await _context.Movie.ToListAsync());
        }
        
        [AllowAnonymous]
        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var movie = await _context.Movie
                .Include(m => m.MovieGenres)!
                .ThenInclude(mg => mg.Genre)
                .Include(m => m.MoviePegis)!
                .ThenInclude(mg => mg.Pegi)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }
        
        // GET: Movies/Create
        public IActionResult Create()
        {

            var createMovieViewModel = new CreateMovieViewModel
            {
                Movie = new Movie(),
                Pegis = _context.Pegi.ToList(),
                Genre = _context.Genre.ToList()
            };
            
            
            return View(createMovieViewModel);
        }
        
        

        
        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Duration,Cast,Director,ImdbScore,ReleaseDate,Is3D,IsSecret,Language,ImageUrl,TrailerUrl")] Movie movie, int[] pegis, int[] genres)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                
                foreach (var pegiId in pegis)
                {
                    var pegi = new MoviePegi
                    {
                        PegiId = pegiId,
                        MovieId = movie.Id
                    };
                    _context.MoviePegi.Add(pegi); 
                    await _context.SaveChangesAsync();       
                }
                
                foreach (var genreId in genres)
                {
                    var genre = new MovieGenre
                    {
                        GenreId = genreId,
                        MovieId = movie.Id
                    };
                    _context.MovieGenre.Add(genre); 
                    await _context.SaveChangesAsync();       
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var editMovieViewModel = new EditMovieViewModel
            {
                Movie = (await _context.Movie.FindAsync(id)),
                Pegis = _context.Pegi.ToList(),
                Genre = _context.Genre.ToList()
            };
            
            if (editMovieViewModel.Movie == null)
            {
                return NotFound();
            }
            return View(editMovieViewModel);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Duration,Cast,Director,ImdbScore,ReleaseDate,Is3D,IsSecret,Language,ImageUrl,TrailerUrl")] Movie movie, int[] pegis, int[] genres)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                    
                    var movieId = _context.Movie.FirstOrDefault(m => m.Title == movie.Title)!.Id;

                    var moviePegiList = _context.MoviePegi.Where(p => p.MovieId == movieId).ToList();
                    
                    foreach (var moviePegi in moviePegiList)
                    {
                        _context.MoviePegi.Remove(moviePegi);
                        await _context.SaveChangesAsync();
                    }
                    
                    var movieGenreList = _context.MovieGenre.Where(p => p.MovieId == movieId).ToList();
                    
                    foreach (var movieGenre in movieGenreList)
                    {
                        _context.MovieGenre.Remove(movieGenre);
                        await _context.SaveChangesAsync();
                    }
                    
                    foreach (var pegiId in pegis)
                    {
                        var pegi = new MoviePegi
                        {
                            PegiId = pegiId,
                            MovieId = movieId
                        };
                        _context.MoviePegi.Add(pegi); 
                        await _context.SaveChangesAsync();       
                    }
                    
                    foreach (var genreId in genres)
                    {
                        var genre = new MovieGenre 
                        {
                            GenreId = genreId,
                            MovieId = movieId
                        };
                        _context.MovieGenre.Add(genre); 
                        await _context.SaveChangesAsync();       
                    }
 }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            return View();
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            _context.Movie.Remove(movie!);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
