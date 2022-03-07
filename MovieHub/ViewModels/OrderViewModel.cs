using MovieHub.Models;

namespace MovieHub.ViewModels;

public class OrderViewModel
{
    public Showtime? showtime { get; set; }

    public List<Showtime>? MovieNext { get; set; }
    public List<Showtime>? MovieNow { get; set; }
}