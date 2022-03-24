using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MovieHub.Controllers;

public class AdminController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }
    
    
    
    
    // GET
    public IActionResult Index()
    {
        return View();
    }
}