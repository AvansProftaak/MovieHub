using MovieHub.Models;

namespace MovieHub.ViewModels;

public class SeatViewModel
{
    public IEnumerable<Seat>? Seats { get; set; }
    public List<Tickettype>? Tickettypes { get; set; }
    
}