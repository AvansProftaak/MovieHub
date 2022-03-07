using System.Collections.Generic;
using MovieHub.Controllers;
using MovieHub.Data;

namespace MovieHub.Models;

public class Tickettype
{
    private readonly ApplicationDbContext _context;
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public double Price { get; set; }
    public string Description { get; set; } = null!;

    public IList<Tickettype> Tickettypes { get; set; }

    public Tickettype()
    {
        
    }

    public decimal PriceCalculations(Tickettype ticket, Movie movie)
    {
        decimal price = 7;

        return price;
    }
    
    public decimal GetNormalPrice()
    {
        IEnumerable<Tickettype> normalTicket = TicketTypeController.GetNormalTicket();

        return Price;
    }
}