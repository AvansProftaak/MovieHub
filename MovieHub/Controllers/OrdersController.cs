using System.Globalization;
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
    public async Task<ActionResult<OrderViewModel>> Index(Movie movie, int showTimeId)
    {
        OrderViewModel orderViewModel = new OrderViewModel();

        orderViewModel.Movie = GetMovie(movie.Id);
        orderViewModel.Tickettypes = TicketTypes(movie.Id);
        orderViewModel.CateringPackages = GetCateringPackages();
        orderViewModel.StartDates = GetStartDates(movie.Id);
        orderViewModel.ShowList = new List<SelectListItem>();
        orderViewModel.PickedShowtime = GetPickedShowtime(showTimeId);

        foreach (var item in GetStartDates(movie.Id))
        {
            var show = new SelectListItem()
            {
                Value = item.Id.ToString(),
                Text = item.StartAt.ToString()
            };

            orderViewModel.ShowList.Add(show);
        }
        
        return View(orderViewModel);
    }

    public List<Showtime> GetStartDates(int movieId)
    {
        var showtimes = _context.Showtime
            .FromSqlRaw("SELECT * FROM \"Showtime\" WHERE \"MovieId\" = {0} ORDER BY \"StartAt\"", movieId).ToList();
        
        var showsThisWeek = new List<Showtime>();
        
        foreach (var show in showtimes)
        {
            if ((show.StartAt >= DateTime.Now) && (show.StartAt.Date <= DateTime.Now.Date.AddDays(7)))
            {
                showsThisWeek.Add(show);
            }
        }

        return showsThisWeek;
    }

    public ActionResult<Showtime> GetPickedShowtime(int showTimeId)
    {
        var show = _context.Showtime!
            .Where(s => s.Id == showTimeId);

        return Json(show);
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

    public List<Tickettype>? TicketTypes(int movieId)
    {
        List<Tickettype>? tickets = GetAllTicketTypes();

        // due to the nature of our calculations we need to hold the normal price after we set it
        // to do this we need this bool ( more explanation in pricecalc function)
        bool normalPriceRaised = false;
        foreach (var ticket in tickets)
        {
            ticket.Price = TicketTypeController.PriceCalculations(ticket, GetMovie(movieId), _context, normalPriceRaised);
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

}