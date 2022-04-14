using MovieHub.Models;

namespace MovieHub.ViewModels;

public class SeatViewModel
{
    public IEnumerable<Seat>? Seats { get; set; }
    public List<Tickettype>? Tickettypes { get; set; }

    public int seatToChange { get; set; }
    public bool changeSeat  { get; set; }
    public int ticketId { get; set; }
    public int showTimeId { get; set; }
}