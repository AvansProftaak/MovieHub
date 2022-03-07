using System.Collections.Generic;

namespace MovieHub.Models;

public class Tickettype
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public double Price { get; set; }
    public string Description { get; set; } = null!;

    public IList<CinemaTickettype>? CinemaTickettypes { get; set; }

    public Tickettype()
    {
        
    }
}