using Microsoft.AspNetCore.Mvc;

namespace MovieHub.Controllers;

public class OrdersController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}