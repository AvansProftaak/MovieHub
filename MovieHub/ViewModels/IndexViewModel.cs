using MovieHub.Models;

namespace MovieHub.ViewModel;

public class IndexViewModel
{

    public Hall? Hall { get; set; }
    public List<Hall>? Halls { get; set; }
    
    public Movie? Movie { get; set; }
    
    public IEnumerable<Showtime>? MovieIndex { get; set; }
    public List<Showtime>? ShowNext { get; set; }
    public List<Showtime>? ShowNow { get; set; }
    
}