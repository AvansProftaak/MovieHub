using MovieHub.Models;

namespace MovieHub.ViewModels;

public class IndexViewModel
{
    public List<Hall>? Halls { get; set; }
    public List<Showtime>? Showtimes { get; set; }
    public List<Movie>? Movies { get; set; }
    public List<MovieRuntime>? MovieRuntimes { get; set; }
}