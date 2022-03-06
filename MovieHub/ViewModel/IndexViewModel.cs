using MovieHub.Models;

namespace MovieHub.ViewModel;

public class IndexViewModel
{

    public Hall? Hall { get; set; }
    public List<Hall>? AllHalls { get; set; }
    
    public Movie? Movie { get; set; }
    
    public IEnumerable<Showtime>? MovieIndex { get; set; }
    public List<Showtime>? MovieNext { get; set; }
    public List<Showtime>? MovieNow { get; set; }


    
}