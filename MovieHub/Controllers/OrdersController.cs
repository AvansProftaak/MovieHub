using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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


    // CHANGED INCOMING SHOWTIME TO MOVIE 
    public Task<ActionResult<OrderViewModel>> Index(int id)
    {
        var orderViewModel = new OrderViewModel
        {
            Movie = GetMovie(id, _context),
            Tickettypes = TicketTypes(id, _context),
            CateringPackages = GetCateringPackages(_context),
            StartDates = GetStartDates(id),
            ShowList = new List<SelectListItem>(),
            MoviePegis = MoviePegis()
        };

        foreach (var show in GetStartDates(id).Select(item => new SelectListItem()
                 {
                     Value = item.Id.ToString(),
                     Text = item.StartAt.ToLocalTime().ToString("dddd dd MMMM yyyy, HH:mm")
                 }))
        {
            orderViewModel.ShowList.Add(show);
        }
        
        return Task.FromResult<ActionResult<OrderViewModel>>(View(orderViewModel));
    }

    public List<Showtime> GetStartDates(int movieId)
    {
        var showtimes = _context.Showtime?
            .FromSqlRaw("SELECT * FROM \"Showtime\" WHERE \"MovieId\" = {0} ORDER BY \"StartAt\"", movieId).ToList();

        return showtimes!.Where(show => (show.StartAt >= DateTime.Now) && (show.StartAt.Date <= DateTime.Now.Date.AddDays(7))).ToList();
    }


    private static List<Tickettype> GetAllTicketTypes(ApplicationDbContext context)
    {
        return context.Tickettype.OrderByDescending(t => t.Price).ToList();
    }

    public static List<CateringPackage> GetCateringPackages(ApplicationDbContext context)
    {
        return context.CateringPackage
            .FromSqlRaw("SELECT * FROM public.\"CateringPackage\"").ToList();
    }

    private static List<Tickettype> TicketTypes(int movieId, ApplicationDbContext context)
    {
        var _context = context; 
        var tickets = GetAllTicketTypes(context);

        // due to the nature of our calculations we need to hold the normal price after we set it
        // to do this we need this bool ( more explanation in pricecalc function)
        var normalPriceRaised = false;
        foreach (var ticket in tickets)
        {
            ticket.Price = TicketTypeController.PriceCalculations(ticket, movieId, _context, normalPriceRaised);
            if (ticket.Name == "Normal")
            {
                normalPriceRaised = true;
            }
        }

        return tickets;
    }

    private static Movie? GetMovie(int id, ApplicationDbContext context)
    {
        return context.Movie
            .Where(m => m.Id.Equals(id)).ToList().FirstOrDefault();
    }

    public List<MoviePegi> MoviePegis()
    {
        return _context.MoviePegi
            .Include(m=> m.Pegi).ToList();
    }


    public static Dictionary <int, decimal> CalculationTicketTypes(int movieId, ApplicationDbContext context)
    {
        var _context = context; 
        var tickets = GetAllTicketTypes(context);

        // due to the nature of our calculations we need to hold the normal price after we set it
        // to do this we need this bool ( more explanation in pricecalc function)
        var normalPriceRaised = false;
        foreach (var ticket in tickets)
        {
            ticket.Price = TicketTypeController.PriceCalculations(ticket, movieId, _context, normalPriceRaised);
            if (ticket.Name == "Normal")
            {
                normalPriceRaised = true;
            }
        }

        var prices = tickets.ToDictionary(ticket => ticket.Id, ticket => ticket.Price);

        foreach (var ticket in tickets)
        {
            _context.Entry(ticket).State = EntityState.Unchanged;
        }

        return prices;
    }
    
    public static Dictionary <int, string> ReturnTicketNames(int movieId, ApplicationDbContext context)
    {
        var tickets = GetAllTicketTypes(context);

        // due to the nature of our calculations we need to hold the normal price after we set it
        // to do this we need this bool ( more explanation in pricecalc function)
        var normalPriceRaised = false;
        foreach (var ticket in tickets)
        {
            ticket.Price = TicketTypeController.PriceCalculations(ticket, movieId, context, normalPriceRaised);
            if (ticket.Name == "Normal")
            {
                normalPriceRaised = true;
            }
        }

        var names = tickets.ToDictionary(ticket => ticket.Id, ticket => ticket.Name);
        foreach (var ticket in tickets)
        {
            context.Entry(ticket).State = EntityState.Unchanged;
        }
        
        
        return names;
    }
    
}