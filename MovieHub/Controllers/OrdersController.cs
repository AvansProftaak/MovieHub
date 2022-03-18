using System.Collections;
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
    public Task<ActionResult<OrderViewModel>> Index(int id)
    {
        var orderViewModel = new OrderViewModel();

        orderViewModel.Movie = GetMovie(id, _context);
        orderViewModel.Tickettypes = TicketTypes(id, _context);
        orderViewModel.CateringPackages = GetCateringPackages(_context);
        orderViewModel.StartDates = GetStartDates(id);
        orderViewModel.ShowList = new List<SelectListItem>();
        orderViewModel.MoviePegis = MoviePegis();

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
    
    
    

    public static List<Tickettype>? GetAllTicketTypes(ApplicationDbContext context)
    {
        return context.Tickettype.ToList();
    }

    public static List<CateringPackage>? GetCateringPackages(ApplicationDbContext context)
    {
        return context.CateringPackage
            .FromSqlRaw("SELECT * FROM public.\"CateringPackage\"").ToList();
    }
 
    public static List<Tickettype>? TicketTypes(int movieId, ApplicationDbContext context)
    {
        ApplicationDbContext _context = context; 
        List<Tickettype>? tickets = GetAllTicketTypes(context);

        // due to the nature of our calculations we need to hold the normal price after we set it
        // to do this we need this bool ( more explanation in pricecalc function)
        bool normalPriceRaised = false;
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
    
    public static Movie? GetMovie(int id, ApplicationDbContext context)
    {
        return context.Movie
            .Where(m => m.Id.Equals(id)).ToList().FirstOrDefault();
    }

    public List<MoviePegi> MoviePegis()
    {
        return _context.MoviePegi
            .Include(m=> m.Pegi).ToList();
    }

}