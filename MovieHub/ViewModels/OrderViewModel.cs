using MovieHub.Models;

namespace MovieHub.ViewModels;

public class OrderViewModel
{
    public Showtime? Showtime { get; set; }

    public List<Tickettype>? Tickettypes { get; set; }
    public List<CateringPackage>? CateringPackages { get; set; }
    public Movie? Movie { get; set; }
    
}