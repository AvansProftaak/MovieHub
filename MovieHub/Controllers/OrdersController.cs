using Microsoft.AspNetCore.Mvc;
using MovieHub.Models;

namespace MovieHub.Controllers;

public class OrdersController : Controller
{
    // GET
    public IActionResult Index(Showtime showtime)
    {
        return View(showtime);
    }
}