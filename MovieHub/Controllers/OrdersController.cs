using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        orderViewModel.Showtime = showtime;
        orderViewModel.Movie = GetMovie(showtime.MovieId);
        orderViewModel.Tickettypes = TicketTypes(showtime.MovieId);
        orderViewModel.CateringPackages = GetCateringPackages();
        
        
        
        return View(orderViewModel);
    }
    
    public List<Tickettype>? GetAllTicketTypes()
    {
        return _context.Set<Tickettype>().ToList();
    }
    
    public List<CateringPackage>? GetCateringPackages()
    {
        return _context.CateringPackage
            .FromSqlRaw("SELECT * FROM public.\"CateringPackage\"").ToList();
    }

    public List<Tickettype>? TicketTypes(int MovieId)
    {
        List<Tickettype>? tickets = GetAllTicketTypes();

        // due to the nature of our calculations we need to hold the normal price after we set it
        // to do this we need this bool ( more explanation in pricecalc function)
        bool normalPriceRaised = false;
        foreach (var ticket in tickets)
        {
            ticket.Price = TicketTypeController.PriceCalculations(ticket, GetMovie(MovieId), _context, normalPriceRaised);
            if (ticket.Name == "Normal")
            {
                normalPriceRaised = true;
            }
        }

        return tickets;

    }
    
    // here we create the order, make tickets, orer and fill the db
    // values i will get from others movieId, showtimeId, ticketsWanted [tickettupeid , quantity], seat [rowNumber, seatnumber], cateringWanted[cateringID, quantity]
    public int PlaceOrdersController(int movieId, int showtimeId,IDictionary<string, string> ticketsWanted["Name of ticket" , "quantity"],IDictionary<int, int> seat["rowNumber", "seatnumber"], IDictionary<int, string> cateringWanted["cateringID", "quantity"])
    {
        // TODO: get the showtime with showtime id
        Showtime showtime = getshowtimeWithShowtimeId;

        OrderViewModel orderViewModel = new OrderViewModel();

        orderViewModel.Showtime = showtime;
        orderViewModel.Movie = GetMovie(showtime.MovieId);
        orderViewModel.Tickettypes = TicketTypes(showtime.MovieId);
        orderViewModel.CateringPackages = GetCateringPackages();

        // create a new order and put order id in var to put in ticket
        Order order = new Order();
        
        foreach (keyvaluepair ticket in ticketsWanted)
        {
            // set counter to loop seat asswell
            // find ticketType with calculated values and create tickets x quantity
            // set orderid in ticket
            // also add a seat to eveyticket
        }
        
        foreach (keyvaluepair ticket in ticketsWanted)
        {
            // set counter to loop seat asswell
            // find ticketType with calculated values and create tickets x quantity
            // set orderid in ticket
            // also add a seat to eveyticket
        }

        return orderid;
    }
    
    public Movie? GetMovie(int id)
    {
        return _context.Movie
            .Where(m => m.Id.Equals(id)).ToList().FirstOrDefault();
    }

}