using Microsoft.AspNetCore.Mvc;

namespace MovieHub.Controllers;

public class BackOfficeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}