using Microsoft.AspNetCore.Mvc;
using MovieHub.Models;
using MovieHub.ViewModels;

namespace MovieHub.Controllers;

public class OrdersController : Controller
{
    // GET

    public async Task<ActionResult<OrderViewModel>> Index(Showtime showtime)
    {
        OrderViewModel indexViewModel = new OrderViewModel();
        
        indexViewModel.MovieIndex = MovieIndex();
        indexViewModel.AllHalls = GetHall();
        indexViewModel.MovieNext = MovieNext();
        indexViewModel.MovieNow = MovieNow();

        return View(showtime);
        return View(indexViewModel);
    }
    
}