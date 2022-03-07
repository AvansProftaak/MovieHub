using MovieHub.Models;

namespace MovieHub.ViewModels;

public class OrderViewModel
{
    public Showtime? showtime { get; set; }

    public IQueryable<Tickettype>? Tickettypes { get; set; }
    public IQueryable<Movie>? Movie { get; set; }
}