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


    // here we create the order, make tickets, orer and fill the db
    // values i will get from others movieId, showtimeId, ticketsWanted [ticketid , quantity], seat [rowNumber, seatnumber], cateringWanted[cateringID, quantity]
    public static int PlaceOrder(ApplicationDbContext context)
    {
        ApplicationDbContext _context = context;
        int movieId = 7;
        int showtimeId = 1837;
        IDictionary<int, string> ticketsWanted = new Dictionary<int, string>();
        ticketsWanted.Add(1, "8");
        ticketsWanted.Add(3, "7.50");

        Showtime showtime = _context.Showtime
            .Where(s => (s.Id >= showtimeId)).FirstOrDefault();
        
        Movie movie = GetMovie(showtime.MovieId, _context);
        List<Tickettype> tickettypes = TicketTypes(showtime.MovieId, context);
        List<CateringPackage> cateringPackages = GetCateringPackages(context);
        //List<Seat> seats = GetSeat();

        // create a new order and put order id in var to put in ticket
        Order order = new Order();

        int counter = 0;
        
        foreach (int key in ticketsWanted.Keys)
        {
            counter += 1;
            decimal ticketPrice = System.Convert.ToDecimal(ticketsWanted[key]);
            Ticket ticketToCreate = new Ticket();
            ticketToCreate.Barcode = 12345;
            //ticketToCreate.Name = tickettypes.Where(t => t.Id  ==key ).FirstOrDefault().Name; 
            ticketToCreate.Name = "Henk"; 
            ticketToCreate.OrderId = order.Id;
            ticketToCreate.Price = ticketPrice;

            // also add a seat to eveyticket
        }

        /*foreach (keyvaluepair catering in cateringWanted)
        {
            CateringPackage createCateringTickets = new CateringPackage();
        }*/

        return order.Id;
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