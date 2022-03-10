﻿using Microsoft.AspNetCore.Mvc;
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