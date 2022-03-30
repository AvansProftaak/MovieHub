using MovieHub.Models;

namespace MovieHub.ViewModels;

public class HallAnalyticsViewModel
{
    public IEnumerable<Hall>? Halls { get; set; }
    public IEnumerable<Showtime>? Showtimes { get; set; }
    public IEnumerable<Movie>? Movies { get; set; }
}