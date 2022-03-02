using MovieHub.Models;

namespace MovieHub.ViewModel;

public class IndexViewModel
{
    public Hall? Hall { get; set; }
    public Movie? Movie { get; set; }
    
    public IEnumerable<Showtime>? MovieIndex { get; set; }
    public IEnumerable<Showtime>? HallIndex { get; set; }
    
    
}