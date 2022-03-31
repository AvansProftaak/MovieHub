using MovieHub.Models;

namespace MovieHub.ViewModels;

public class IndexViewModel
{
    public Hall? Hall { get; set; }
    public List<Hall>? Halls { get; set; }
    public Movie? Movie { get; set; }
    public List<Movie>? Movies { get; set; }
    public List<Showtime>? ShowNext { get; set; }
    public List<Showtime>? ShowNow { get; set; }
    public List<MovieRuntime>? MovieRuntimes { get; set; }

}