using Microsoft.AspNetCore.Mvc;
using MovieHub.Data;
using MovieHub.Models;
using MovieHub.ViewModels;

namespace MovieHub.Controllers;

public class OrdersController : Controller
{
    private readonly ApplicationDbContext _context;
    public OrdersController(ApplicationDbContext context)
    {
        _context = context;
    }
    // GET

    public async Task<ActionResult<OrderViewModel>> Index(Showtime showtime)
    {
        OrderViewModel orderViewModel = new OrderViewModel();

        orderViewModel.showtime = showtime;
        orderViewModel.Movie = GetMovie(showtime.MovieId);
        orderViewModel.Tickettypes = TicketTypes(showtime.MovieId);
        
        
        
        return View(orderViewModel);
    }
    
    public List<Tickettype>? GetAllTicketTypes()
    {
        return _context.Set<Tickettype>().ToList();
    }

    public List<Tickettype>? TicketTypes(int MovieId)
    {
        List<Tickettype>? tickets = GetAllTicketTypes();

        foreach (var ticket in tickets)
        {
            ticket.Price = TicketTypeController.PriceCalculations(ticket, GetMovie(MovieId), _context);
        }

        return tickets;

    }
    
    public Movie? GetMovie(int id)
    {
        return _context.Movie
            .Where(m => m.Id.Equals(id)).ToList().FirstOrDefault();
    }

}