#nullable disable

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
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var movie = await _context.Movie
                .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .Include(m => m.MoviePegis)
                .ThenInclude(mg => mg.Pegi)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            };
            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View(new CreateMovieViewModel
            {
                Movie = new Movie(),
                Pegis = _context.Pegi.ToList(),
                Genres = _context.Genre.ToList()
            });
        }
      
        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Duration,Cast,Director,ImdbScore,ReleaseDate,Is3D,IsSecret,Language,ImageUrl,TrailerUrl")] Movie movie, int[] pegis, int[] genres)
        {
            
            Movie movieToSave = new Movie
            {
                Title = movie.Title,
                Description = movie.Description,
                Duration = movie.Duration,
                Cast = movie.Cast,
                Director = movie.Director,
                ImdbScore = movie.ImdbScore,
                ReleaseDate = movie.ReleaseDate,
                Is3D = movie.Is3D,
                IsSecret = movie.IsSecret,
                Language = movie.Language,
                ImageUrl = movie.ImageUrl,
                TrailerUrl = movie.TrailerUrl
            };

            if (ModelState.IsValid)
            {
                
                //_context.Movie.Remove(movie);
                _context.Add(movieToSave);
                await _context.SaveChangesAsync();

                foreach (var pegiId in pegis)
                {
                    var pegi = new MoviePegi
                    {
                        PegiId = pegiId,
                        MovieId = movieToSave.Id
                    };
                    _context.MoviePegi.Add(pegi); 
                    await _context.SaveChangesAsync();       
                }
                
                foreach (var genreId in genres)
                {
                    var genre = new MovieGenre
                    {
                        GenreId = genreId,
                        MovieId = movieToSave.Id
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
            
            var movie = await _context.Movie.FindAsync(id);
            
            if (movie == null)
            {
                return NotFound();
            }
            
            return View(new CreateMovieViewModel
            {
                Movie = movie,
                Pegis = _context.Pegi.ToList(),
                Genres = _context.Genre.ToList()
            });
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Description,Duration,Cast,Director,ImdbScore,ReleaseDate,Is3D,IsSecret,Language,ImageUrl,TrailerUrl")] Movie movie, int[] pegis, int[] genres)
        {
            movie.Id = id;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                    
                    var pegiList = _context.MoviePegi.Where(p => p.MovieId == movie.Id).ToList();
                    
                    foreach (var pegi in pegiList)
                    {
                        _context.MoviePegi.Remove(pegi);
                        await _context.SaveChangesAsync();
                    }
                    
                    var genreList = _context.MovieGenre.Where(p => p.MovieId == movie.Id).ToList();
                    
                    foreach (var genre in genreList)
                    {
                        _context.MovieGenre.Remove(genre);
                        await _context.SaveChangesAsync();
                    }
                    
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
            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}