using MovieHub.Models;

namespace MovieHub.ViewModel;

public class MovieViewModel
{
    public Movie Movie { get; set; }
    public IEnumerable<MovieGenre>? MovieGenres { get; set; }
    public IEnumerable<MoviePegi>? MoviePegis { get; set; }
}