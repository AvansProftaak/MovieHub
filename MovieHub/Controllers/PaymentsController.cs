using Microsoft.AspNetCore.Mvc;

namespace MovieHub.Controllers;

public class PaymentsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}