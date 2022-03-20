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
            Movie = GetMovie(id),
            Tickettypes = TicketTypes(id),
            CateringPackages = GetCateringPackages(),
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
    
    public List<Tickettype> GetAllTicketTypes()
    {
        return _context.Tickettype.OrderByDescending(m => m.Price).ToList();
    }
    
    public List<CateringPackage> GetCateringPackages()
    {
        return _context.CateringPackage
            .FromSqlRaw("SELECT * FROM public.\"CateringPackage\"").ToList();
    }

    public List<Tickettype> TicketTypes(int movieId)
    {
        
        
        var tickets = GetAllTicketTypes();
    
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
    
    public Movie? GetMovie(int id)
    {
        return _context.Movie
            .Where(m => m.Id.Equals(id)).ToList().FirstOrDefault();
    }

    /*public Order? PlaceOrder()
    {
        // here we need to create the tickets - > will be done in ticketController
        // the ticket controller wil bu used to save tickets and arangement tickets
        // put the tickets in order and return order
        
        
        return "Order Placed";
    }*/

    public List<MoviePegi> MoviePegis()
    {
        return _context.MoviePegi
            .Include(m=> m.Pegi).ToList();
    }

}