using MovieHub.Models;

namespace MovieHub.ViewModels;

public class OrderViewModel
{
    public Showtime? showtime { get; set; }

    public IQueryable? Tickettypes { get; set; }
    public Movie? movie { get; set; }
}