using MovieHub.Models;

namespace MovieHub.ViewModel;

public class IndexViewModel
{
    public Hall hall { get; set; }
    public Movie movie { get; set; }
    public IEnumerable<Showtime> showtime { get; set; }
    public IEnumerable<Showtime> rob { get; set; }
    
    
}