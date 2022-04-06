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
           return View(await GetMoviesAsync());
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
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Duration,Cast,Director,ImdbScore,ReleaseDate,Is3D,IsSecret,Language,ImageUrl,TrailerUrl")] Movie movie, int[] pegis, int[] genres)
        {
            if (ModelState.IsValid)
            {
                var newMovie = await CreateMovieAsync(movie);
                await CreatePegisAsync(newMovie.Id, pegis);
                await CreateGenresAsync(newMovie.Id, genres);
                
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
                _context.Update(movie);
                await _context.SaveChangesAsync();
                
                await DeletePegisGenresAsync(id);
                await CreatePegisAsync(id, pegis);
                await CreateGenresAsync(id, genres);
                
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<Movie> CreateMovieAsync(Movie movie)
        {
            var result = new Movie
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
            
            _context.Add(result);
            await _context.SaveChangesAsync();
            
            return result;
        }
        
        public async Task CreatePegisAsync(int movieId, int[] pegis)
        {
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
        }
        
        public async Task CreateGenresAsync(int movieId, int[] genres)
        {
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
        
        public async Task DeletePegisGenresAsync(int movieId)
        {
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
        }
        
        
        

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
        
        [HttpGet]
        public async Task<Movie> GetMovieAsync(int id)
        {
            return await _context.Movie.FirstAsync(m => m.Id == id);
        }

        [HttpGet]
        public async Task<IList<Movie>> GetMoviesAsync()
        {
            return await _context.Movie.OrderBy(m => m.ReleaseDate).ToListAsync();
        }
    }
}
