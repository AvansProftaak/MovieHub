using MovieHub.Models;

namespace MovieHub.ViewModels;

public class MovieViewModel
{
    public Movie Movie { get; set; }
    public IEnumerable<MovieGenre>? MovieGenres { get; set; }
    public IEnumerable<MoviePegi>? MoviePegis { get; set; }
}