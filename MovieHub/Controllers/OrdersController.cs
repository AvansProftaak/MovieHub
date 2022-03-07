using Microsoft.AspNetCore.Mvc;
using MovieHub.Models;
using MovieHub.ViewModels;

namespace MovieHub.Controllers;

public class OrdersController : Controller
{
    // GET

    public async Task<ActionResult<OrderViewModel>> Index(Showtime showtime)
    {
        OrderViewModel orderViewModel = new OrderViewModel();
        
        orderViewModel.showtime = showtime;
        orderViewModel.Tickettypes = TicketTypeController.GetAll();
        
        return View(orderViewModel);
    }
    
    
    
}