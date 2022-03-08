using System.Collections.Generic;
using MovieHub.Controllers;
using MovieHub.Data;

namespace MovieHub.Models;

public class Tickettype
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Description { get; set; } = null!;
    
    public int Quantity { get; set; }

    public IList<Tickettype> Tickettypes { get; set; }

    public Tickettype()
    {
        
    }

    
}