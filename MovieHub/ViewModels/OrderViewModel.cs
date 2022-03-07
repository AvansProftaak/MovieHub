using MovieHub.Models;

namespace MovieHub.ViewModels;

public class OrderViewModel
{
    public Showtime? showtime { get; set; }

    public List<Tickettype>? Tickettypes { get; set; }
}