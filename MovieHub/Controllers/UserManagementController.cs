using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MovieHub.Controllers;

[Authorize (Roles = "Admin")]
public class UserManagementController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Edit()
    {
        return View();
    }
}