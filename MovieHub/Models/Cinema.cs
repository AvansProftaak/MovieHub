using System.Collections.Generic;

namespace MovieHub.Models;

public class Cinema
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? FacebookUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public string? TwitterUrl { get; set; }
    public string? YoutubeUrl { get; set; }
    
    public IList<CinemaTickettype>? CinemaTickettypes { get; set; }
    public IList<CinemaMovie>? CinemaMovies { get; set; }

    
    public Cinema()
    {
        
    }
}